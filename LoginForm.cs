using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using WinFormApiGMPKlik.Properties;

namespace WinFormApiGMPKlik.Forms
{
    public partial class LoginForm : Form
    {
        private Settings _s => Settings.Default;

        public LoginForm()
        {
            InitializeComponent();
        }

        // ── Load: isi form kalau RememberMe aktif ──────────────────
        private void LoginForm_Load_1(object sender, EventArgs e)
        {
            if (_s.RememberMe)
            {
                txtEmail.Text = _s.SavedEmail;
                txtPassword.Text = ApiHelper.DecryptPassword(_s.SavedPassword);
                chkRemember.Checked = true;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e) { }

        // ── Tombol Batal ───────────────────────────────────────────
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ── Tombol Login ───────────────────────────────────────────
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Email dan password tidak boleh kosong.", "Perhatian");
                return;
            }

            btnLogin.Enabled = false;
            lblStatus.Text = "Status: Memeriksa koneksi...";
            lblStatus.ForeColor = System.Drawing.Color.Blue;
            this.Refresh();

            try
            {
                // 1. Cek koneksi
                if (!await ApiHelper.CheckInternetConnectionAsync())
                {
                    lblStatus.Text = "Status: Tidak ada koneksi internet";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    MessageBox.Show("Tidak ada koneksi internet.", "Koneksi Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lblStatus.Text = "Status: Sedang login...";
                this.Refresh();

                // 2. Kirim request login
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(_s.ApiBaseUrl.TrimEnd('/')),
                    Timeout = TimeSpan.FromSeconds(30)
                };

                var payload = JsonSerializer.Serialize(new
                {
                    usernameOrEmail = txtEmail.Text.Trim(),
                    password = txtPassword.Text,
                    rememberMe = chkRemember.Checked
                });
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/api/v1/users/login", content);
                var body = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"[Login] Status: {response.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"[Login] Body: {body}");

                if (!response.IsSuccessStatusCode)
                {
                    lblStatus.Text = $"Status: Login gagal ({(int)response.StatusCode})";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    try
                    {
                        var err = JsonSerializer.Deserialize<ApiResponse<object>>(body,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        MessageBox.Show($"Login gagal: {err?.Message ?? body}", "Error");
                    }
                    catch { MessageBox.Show($"Login gagal: {body}", "Error"); }
                    return;
                }

                // 3. Parse response
                var loginResp = JsonSerializer.Deserialize<ApiResponse<LoginResponseDto>>(body,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (loginResp?.Data == null || string.IsNullOrEmpty(loginResp.Data.Token))
                {
                    lblStatus.Text = "Status: Token tidak diterima";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    MessageBox.Show("Login gagal. Token tidak diterima dari server.", "Error");
                    return;
                }

                // 4. ══ SIMPAN KE SETTINGS ══════════════════════════════
                //    JANGAN panggil Reload() setelah ini sebelum Close()!
                _s.AccessToken = loginResp.Data.Token;
                _s.RefreshToken = loginResp.Data.RefreshToken ?? string.Empty;
                _s.TokenExpiry = loginResp.Data.ExpiresAt.ToString("O");
                _s.CurrentUserId = loginResp.Data.User?.Id ?? string.Empty;
                _s.CurrentUserRoles = string.Join(",", loginResp.Data.User?.Roles ?? new List<string>());
                _s.CurrentUserEmail = loginResp.Data.User?.Email ?? txtEmail.Text.Trim();

                // RememberMe
                _s.RememberMe = chkRemember.Checked;
                if (chkRemember.Checked)
                {
                    _s.SavedEmail = txtEmail.Text.Trim();
                    _s.SavedPassword = ApiHelper.EncryptPassword(txtPassword.Text);
                }
                else
                {
                    _s.SavedEmail = string.Empty;
                    _s.SavedPassword = string.Empty;
                }

                // Save ke disk
                _s.Save();
                MessageBox.Show($"Token dibaca: '{_s.AccessToken}'", "Debug Token");  // ← aktifkan ini
                // 5. Verifikasi hasil simpan (TANPA Reload — baca dari memory)
                System.Diagnostics.Debug.WriteLine($"[Login] Token tersimpan: {_s.AccessToken?.Substring(0, Math.Min(20, _s.AccessToken?.Length ?? 0))}...");
                System.Diagnostics.Debug.WriteLine($"[Login] TokenExpiry: {_s.TokenExpiry}");
                System.Diagnostics.Debug.WriteLine($"[Login] Roles: {_s.CurrentUserRoles}");
                System.Diagnostics.Debug.WriteLine($"[Login] UserId: {_s.CurrentUserId}");

                if (string.IsNullOrEmpty(_s.AccessToken))
                {
                    MessageBox.Show("Gagal menyimpan session. Coba lagi.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 6. Selesai
                lblStatus.Text = "Status: Login berhasil!";
                lblStatus.ForeColor = System.Drawing.Color.Green;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (HttpRequestException httpEx)
            {
                lblStatus.Text = "Status: Koneksi error";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show($"Gagal terhubung ke server.\nDetail: {httpEx.Message}",
                    "Koneksi Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Status: Error";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show($"Error: {ex.Message}\n\n{ex.StackTrace}", "Error");
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }

        // ── Remember Me ────────────────────────────────────────────
        private void chkRemember_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRemember.Checked)
            {
                if (!string.IsNullOrEmpty(txtEmail.Text) && !string.IsNullOrEmpty(txtPassword.Text))
                {
                    _s.SavedEmail = txtEmail.Text.Trim();
                    _s.SavedPassword = ApiHelper.EncryptPassword(txtPassword.Text);
                    _s.RememberMe = true;
                    _s.Save();
                    lblStatus.Text = "Status: Kredensial tersimpan ✓";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblStatus.Text = "Status: Isi email & password terlebih dahulu";
                    lblStatus.ForeColor = System.Drawing.Color.Orange;
                    chkRemember.Checked = false;
                }
            }
            else
            {
                _s.SavedEmail = string.Empty;
                _s.SavedPassword = string.Empty;
                _s.RememberMe = false;
                _s.Save();
                lblStatus.Text = "Status: Kredensial dihapus";
                lblStatus.ForeColor = System.Drawing.Color.Orange;
            }
        }

        // ── Input Guards ───────────────────────────────────────────
        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    MessageBox.Show("Email tidak boleh kosong!", "Peringatan",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus(); txtEmail.SelectAll(); return;
                }
                this.SelectNextControl(txtEmail, true, true, true, true);
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                btnLogin_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                this.SelectNextControl(txtPassword, true, true, true, true);
            }
        }

        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar)) e.Handled = true;
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar)) e.Handled = true;
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if (txtEmail.Text.Contains(" "))
            {
                int pos = txtEmail.SelectionStart;
                txtEmail.Text = txtEmail.Text.Replace(" ", "");
                txtEmail.SelectionStart = Math.Max(0, pos - 1);
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text.Contains(" "))
            {
                int pos = txtPassword.SelectionStart;
                txtPassword.Text = txtPassword.Text.Replace(" ", "");
                txtPassword.SelectionStart = Math.Max(0, pos - 1);
            }
        }
    }
}