using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinFormApiGMPKlik.Properties;

namespace WinFormApiGMPKlik.Forms
{
    public partial class DashboardForm : Form
    {
        private System.Windows.Forms.Timer _countdownTimer; private DateTime _tokenExpiry;
        private TimeSpan _timeRemaining;

        public DashboardForm()
        {
            InitializeComponent();
            InitializeTimer();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            // Cek apakah sudah login
            if (string.IsNullOrEmpty(Settings.Default.AccessToken))
            {
                MessageBox.Show("Silakan login terlebih dahulu!", "Akses Ditolak",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            // Parse expiry time
            if (!DateTime.TryParse(Settings.Default.TokenExpiry, out _tokenExpiry))
            {
                _tokenExpiry = DateTime.UtcNow.AddHours(1); // Default 1 jam
            }

            UpdateUserInfo();
            StartCountdown();
        }

        private void InitializeTimer()
        {
            _countdownTimer = new System.Windows.Forms.Timer(); _countdownTimer.Interval = 1000; // 1 detik
            _countdownTimer.Tick += CountdownTimer_Tick;
        }

        private void StartCountdown()
        {
            _countdownTimer.Start();
            UpdateCountdownDisplay();
        }

        private async void CountdownTimer_Tick(object sender, EventArgs e)
        {
            _timeRemaining = _tokenExpiry - DateTime.UtcNow;

            if (_timeRemaining.TotalSeconds <= 300) // 5 menit sebelum expired
            {
                // Coba refresh token secara silent
                if (ApiHelper.IsTokenExpired() || _timeRemaining.TotalSeconds <= 0)
                {
                    _countdownTimer.Stop();

                    // Coba refresh dulu, jika gagal baru logout
                    var refreshed = await ApiHelper.RefreshTokenAsync();

                    if (refreshed)
                    {
                        // Update expiry time dan lanjutkan session
                        if (DateTime.TryParse(Settings.Default.TokenExpiry, out _tokenExpiry))
                        {
                            StartCountdown();
                            UpdateUserInfo();
                            return;
                        }
                    }

                    // Jika refresh gagal, baru logout
                    TokenExpired();
                    return;
                }
            }

            UpdateCountdownDisplay();
        }

        private void UpdateCountdownDisplay()
        {
            lblCountdown.Text = $"Session Expires In: {_timeRemaining.Hours:D2}:{_timeRemaining.Minutes:D2}:{_timeRemaining.Seconds:D2}";

            // Warning jika kurang dari 5 menit
            if (_timeRemaining.TotalMinutes <= 5)
            {
                lblCountdown.ForeColor = Color.Red;
                lblCountdown.Font = new Font(lblCountdown.Font, FontStyle.Bold);
            }
            else
            {
                lblCountdown.ForeColor = Color.SteelBlue;
                lblCountdown.Font = new Font(lblCountdown.Font, FontStyle.Regular);
            }
        }

        private void UpdateUserInfo()
        {
            var roles = Settings.Default.CurrentUserRoles?.Split(',').ToList() ?? new List<string>();
            var isSuperAdmin = roles.Contains("SuperAdmin");

            lblUserInfo.Text = $"User: {Settings.Default.CurrentUserId}";
            lblRoleInfo.Text = $"Role: {string.Join(", ", roles)}";
            lblEmailInfo.Text = $"Email: {Settings.Default.SavedEmail}";

            // Style untuk SuperAdmin
            if (isSuperAdmin)
            {
                lblRoleInfo.ForeColor = Color.Gold;
                lblRoleInfo.Font = new Font(lblRoleInfo.Font, FontStyle.Bold);
                lblSuperAdminBadge.Visible = true;
            }
            else
            {
                lblSuperAdminBadge.Visible = false;
            }

            // Update status bar
            lblStatus.Text = $"Login sebagai: {Settings.Default.CurrentUserId} | Role: {string.Join(", ", roles)}";
        }

        private void TokenExpired()
        {
            MessageBox.Show("Session telah expired. Silakan login kembali.", "Session Expired",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            ClearSession();
            this.Hide();

            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Restart dashboard dengan token baru
                    var newDashboard = new DashboardForm();
                    newDashboard.ShowDialog();
                }
            }

            this.Close();
        }

        private void ClearSession()
        {
            Settings.Default.AccessToken = string.Empty;
            Settings.Default.RefreshToken = string.Empty;
            Settings.Default.CurrentUserId = string.Empty;
            Settings.Default.CurrentUserRoles = string.Empty;
            Settings.Default.TokenExpiry = string.Empty;
            // Jangan hapus RememberMe dan SavedEmail agar bisa auto-fill
            Settings.Default.Save();
        }

        private void btnUserManagement_Click(object sender, EventArgs e)
        {
            using (var userForm = new UserManagementForm())
            {
                userForm.ShowDialog();
            }
        }

        private void btnDaftarMandiri_Click(object sender, EventArgs e)
        {
            using (var daftarForm = new DaftarMandiri())
            {
                daftarForm.ShowDialog();
            }
        }

        private void btnPengaturan_Click(object sender, EventArgs e)
        {
            using (var settingForm = new PengaturanKoneksi())
            {
                settingForm.ShowDialog();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Yakin ingin logout?", "Konfirmasi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _countdownTimer.Stop();
                ClearSession();
                this.DialogResult = DialogResult.OK; // Trigger login form lagi
                this.Close();
            }
        }

        private void DashboardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _countdownTimer?.Stop();
            _countdownTimer?.Dispose();
        }
    }
}