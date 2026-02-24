using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormApiGMPKlik.Properties;
using WinFormApiGMPKlik.Services;

namespace WinFormApiGMPKlik.Forms
{
    public partial class UserManagementForm : Form
    {
        private List<UserViewModel> _users = new List<UserViewModel>();
        private List<BranchViewModel> _branches = new List<BranchViewModel>();
        private string _currentUserId = string.Empty;
        private bool _isCurrentUserSuperAdmin = false;
        private bool _isCurrentUserAdmin = false;
        private PaginationMetadata? _paginationMetadata;
        private int _currentPage = 1;
        private int _pageSize = 10;
        private UserFilterModel _currentFilter = new UserFilterModel();
        private Settings _s => Settings.Default;
        private IUserService _userService;

        public UserManagementForm()
        {
            InitializeComponent();
        }

        #region Form Load & Initialization
        private async void UserManagementForm_Load(object sender, EventArgs e)
        {
            try
            {
                
                ShowLoading(true);
                Settings.Default.Reload();
                await Task.Delay(100); // Beri waktu sebentar (opsional, tapi membantu)

                // PASTIKAN reload di sini juga
                //ApiHelper.EnsureSettingsLoaded();

                // Cek token sebelum load data
                var token = Settings.Default.AccessToken;
                MessageBox.Show($"Token dibaca: '{token}'", "Debug Token");  // ← aktifkan ini
                //return;
                if (string.IsNullOrEmpty(token))
                {
                    MessageBox.Show("Session tidak ditemukan. Token kosong atau belum disimpan.\nSilakan login kembali.",
                        "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //HandleSessionExpired();
                    return;
                }

                // Debug: Cek token (hapus setelah fix)
                //System.Diagnostics.Debug.WriteLine($"Token: {_s.AccessToken?.Substring(0, Math.Min(20, _s.AccessToken?.Length ?? 0))}...");
                //System.Diagnostics.Debug.WriteLine($"TokenExpiry: {_s.TokenExpiry}");
                if (!ApiHelper.VerifyTokenFormat(token))
                {
                    MessageBox.Show("Format token tidak valid. Silakan login kembali.",
                        "Token Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //HandleSessionExpired();
                    return;
                }


                // Inisialisasi komponen
                InitializeDefaultValues();
                GetCurrentUserInfo();

                // Cek koneksi internet
                if (!await CheckInternetConnectionAsync())
                {
                    MessageBox.Show("Tidak ada koneksi internet. Pastikan Anda terhubung ke jaringan.",
                        "Koneksi Terputus", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Load data branches
                await LoadBranchesAsync();

                // Load users dengan error handling lebih baik
                await LoadUsersAsync(1);

                // Setup UI berdasarkan role
                SetupUIBasedOnRole();
                UpdateCurrentUserLabel();

                ShowLoading(false);
                UpdateStatus("Ready");
            }
            catch (Exception ex)
            {
                ShowLoading(false);
                MessageBox.Show($"Error saat memuat form: {ex.Message}\n\nStack: {ex.StackTrace}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
        private void InitializeDefaultValues()
        {
            cmbPageSize.SelectedItem = "10";
            cmbSortBy.SelectedItem = "CreatedAt";
            cmbIsActive.SelectedItem = "Active Only";
            cmbIsDeleted.SelectedItem = "Active Only";
            cmbRole.SelectedItem = "All";

            // Default date pickers tidak checked
            dtpCreatedFrom.Checked = false;
            dtpCreatedTo.Checked = false;
        }

        private void GetCurrentUserInfo()
        {
            try
            {
                _currentUserId = Settings.Default.CurrentUserId;
                var roles = Settings.Default.CurrentUserRoles?.Split(',').ToList() ?? new List<string>();
                _isCurrentUserSuperAdmin = roles.Contains("SuperAdmin");
                _isCurrentUserAdmin = roles.Contains("Admin") || _isCurrentUserSuperAdmin;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error parsing user info: {ex.Message}");
            }
        }

        private void SetupUIBasedOnRole()
        {
            // SuperAdmin bisa lihat deleted users
            if (!_isCurrentUserSuperAdmin)
            {
                cmbIsDeleted.Enabled = false;
                btnRestore.Visible = false;
                btnResetPassword.Visible = false;
            }
            else
            {
                cmbIsDeleted.Enabled = true;
                btnRestore.Visible = true;
                btnResetPassword.Visible = true;
            }

            // Admin bisa export
            btnExport.Visible = _isCurrentUserAdmin;
        }

        private void UpdateCurrentUserLabel()
        {
            var roles = Settings.Default.CurrentUserRoles?.Split(',').ToList() ?? new List<string>();
            lblCurrentUser.Text = $"{Settings.Default.CurrentUserEmail} ({string.Join(", ", roles)})";
        }

        #endregion

        #region Internet Connection Check

        private async Task<bool> CheckInternetConnectionAsync()
        {
            try
            {
                UpdateStatus("Mengecek koneksi internet...");
                lblConnectionStatus.Text = "🟡 Mengecek...";
                lblConnectionStatus.ForeColor = System.Drawing.Color.Orange;

                var isConnected = await ApiHelper.CheckInternetConnectionAsync();

                if (isConnected)
                {
                    lblConnectionStatus.Text = "🟢 Terhubung";
                    lblConnectionStatus.ForeColor = System.Drawing.Color.Green;
                    return true;
                }
                else
                {
                    lblConnectionStatus.Text = "🔴 Terputus";
                    lblConnectionStatus.ForeColor = System.Drawing.Color.Red;
                    return false;
                }
            }
            catch
            {
                lblConnectionStatus.Text = "🔴 Error";
                lblConnectionStatus.ForeColor = System.Drawing.Color.Red;
                return false;
            }
        }

        #endregion

        #region Data Loading

        private async Task LoadBranchesAsync()
        {
            try
            {
                // Simulasi load branches (sesuaikan dengan endpoint Anda)
                // Untuk sementara tambahkan item manual
                cmbBranch.Items.Clear();
                cmbBranch.Items.Add("All");

                // Jika ada endpoint branches, uncomment:
                /*
                var response = await ApiHelper.ExecuteWithRefreshAsync(client =>
                    client.GetAsync("/api/v1/branches"));
                
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ApiResponse<List<BranchViewModel>>>(json);
                    if (result?.Data != null)
                    {
                        _branches = result.Data;
                        foreach (var branch in _branches)
                        {
                            cmbBranch.Items.Add($"{branch.Id} - {branch.Name}");
                        }
                    }
                }
                */

                cmbBranch.Items.Add("1 - Head Office");
                cmbBranch.Items.Add("2 - Branch A");
                cmbBranch.SelectedItem = "All";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading branches: {ex.Message}");
            }
        }

        private async Task LoadUsersAsync(int page)
        {
            try
            {
                ShowLoading(true);
                UpdateStatus("Loading users...");

                var query = new UserQueryParams
                {
                    Page = page,
                    PageSize = _pageSize,
                    Search = txtSearch.Text?.Trim(),
                    SortBy = cmbSortBy.SelectedItem?.ToString() ?? "CreatedAt",
                    SortDescending = chkSortDescending.Checked,
                    IsActive = GetFilterValue(cmbIsActive),
                    IsDeleted = _isCurrentUserSuperAdmin ? GetFilterValue(cmbIsDeleted) : null,
                    Role = cmbRole.SelectedItem?.ToString() != "All" ? cmbRole.SelectedItem?.ToString() : null,
                    CreatedFrom = dtpCreatedFrom.Checked ? dtpCreatedFrom.Value : null,
                    CreatedTo = dtpCreatedTo.Checked ? dtpCreatedTo.Value : null
                };

                var (users, metadata) = await _userService.GetUsersAsync(query);

                _users = users;
                _paginationMetadata = metadata;

                // Filter SuperAdmin protection
                if (!_isCurrentUserSuperAdmin)
                {
                    _users = _users.Where(u => !u.Roles.Contains("SuperAdmin")).ToList();
                }

                BindDataGrid();
                UpdatePaginationControls();
                UpdateTotalRecords();
            }
            catch (UnauthorizedAccessException)
            {
                HandleSessionExpired();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowLoading(false);
                UpdateStatus("Ready");
            }
        }

        private bool? GetFilterValue(ComboBox cmb)
        {
            var selected = cmb.SelectedItem?.ToString();
            return selected == "Active Only" ? true :
                   selected == "Inactive Only" ? false : null;
        }

        private void BindDataGrid()
        {
            dgvUsers.DataSource = null;

            // Buat binding list untuk better performance
            var bindingList = new System.ComponentModel.BindingList<UserViewModel>(_users);
            dgvUsers.DataSource = bindingList;

            // Sembunyikan kolom yang tidak perlu
            if (dgvUsers.Columns["Id"] != null)
                dgvUsers.Columns["Id"].Visible = false;

            if (dgvUsers.Columns["Profile"] != null)
                dgvUsers.Columns["Profile"].Visible = false;

            // Format header
            var columnHeaders = new Dictionary<string, string>
            {
                {"Username", "Username"},
                {"Email", "Email"},
                {"FullName", "Nama Lengkap"},
                {"PhoneNumber", "Telepon"},
                {"IsActive", "Aktif"},
                {"IsDeleted", "Dihapus"},
                {"CreatedAt", "Tanggal Daftar"},
                {"Roles", "Role"},
                {"BranchName", "Cabang"}
            };

            foreach (var header in columnHeaders)
            {
                if (dgvUsers.Columns[header.Key] != null)
                    dgvUsers.Columns[header.Key].HeaderText = header.Value;
            }

            // Format kolom tanggal
            if (dgvUsers.Columns["CreatedAt"] != null)
            {
                dgvUsers.Columns["CreatedAt"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvUsers.Columns["CreatedAt"].Width = 130;
            }

            // Warna untuk row yang dihapus
            foreach (DataGridViewRow row in dgvUsers.Rows)
            {
                if (row.DataBoundItem is UserViewModel user && user.IsDeleted)
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                    row.DefaultCellStyle.ForeColor = System.Drawing.Color.DarkRed;
                }
            }
        }

        private void UpdatePaginationControls()
        {
            if (_paginationMetadata == null) return;

            lblPageInfo.Text = $"Halaman {_paginationMetadata.Page} dari {_paginationMetadata.TotalPages}";

            btnFirst.Enabled = _paginationMetadata.HasPrevious;
            btnPrevious.Enabled = _paginationMetadata.HasPrevious;
            btnNext.Enabled = _paginationMetadata.HasNext;
            btnLast.Enabled = _paginationMetadata.HasNext;
        }

        private void UpdateTotalRecords()
        {
            lblTotalRecords.Text = $"Total: {_paginationMetadata?.TotalCount ?? 0} records";
        }

        #endregion

        #region Filter & Search Events

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await LoadUsersAsync(1);
        }

        private async void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbBranch.SelectedItem = "All";
            cmbIsActive.SelectedItem = "Active Only";
            cmbIsDeleted.SelectedItem = "Active Only";
            cmbRole.SelectedItem = "All";
            dtpCreatedFrom.Checked = false;
            dtpCreatedTo.Checked = false;
            cmbSortBy.SelectedItem = "CreatedAt";
            chkSortDescending.Checked = true;

            await LoadUsersAsync(1);
        }

        #endregion

        #region Pagination Events

        private async void btnFirst_Click(object sender, EventArgs e)
        {
            if (_paginationMetadata?.HasPrevious == true)
                await LoadUsersAsync(1);
        }

        private async void btnPrevious_Click(object sender, EventArgs e)
        {
            if (_paginationMetadata?.HasPrevious == true)
                await LoadUsersAsync(_currentPage - 1);
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            if (_paginationMetadata?.HasNext == true)
                await LoadUsersAsync(_currentPage + 1);
        }

        private async void btnLast_Click(object sender, EventArgs e)
        {
            if (_paginationMetadata?.HasNext == true)
                await LoadUsersAsync(_paginationMetadata.TotalPages);
        }

        private async void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPageSize.SelectedItem != null)
            {
                _pageSize = int.Parse(cmbPageSize.SelectedItem.ToString());
                _currentPage = 1;
                await LoadUsersAsync(1);
            }
        }

        #endregion

        #region CRUD Operations

        private void dgvUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsers?.CurrentRow?.DataBoundItem is not UserViewModel selectedUser)
            {
                ClearDetailForm();
                return;
            }

            DisplayUserDetail(selectedUser);
        }

        private void DisplayUserDetail(UserViewModel user)
        {
            var isSuperAdmin = user.Roles.Contains("SuperAdmin");
            var isDeleted = user.IsDeleted;

            // Fill form
            txtEditUsername.Text = user.Username;
            txtEditEmail.Text = user.Email;
            txtEditName.Text = user.FullName ?? string.Empty;
            txtEditPhone.Text = user.PhoneNumber ?? string.Empty;
            chkEditIsActive.Checked = user.IsActive;
            lblUserRoles.Text = string.Join(", ", user.Roles);

            // Warning untuk SuperAdmin
            lblWarning.Visible = isSuperAdmin;
            if (isSuperAdmin)
            {
                lblWarning.Text = "⚠️ SUPERADMIN - Protected Account";
                btnDelete.Enabled = false;
                chkEditIsActive.Enabled = _isCurrentUserSuperAdmin && user.Id == _currentUserId;
            }
            else
            {
                btnDelete.Enabled = _isCurrentUserAdmin && !isDeleted;
                chkEditIsActive.Enabled = _isCurrentUserAdmin;
            }

            // Info untuk deleted user
            lblDeletedInfo.Visible = isDeleted;
            btnRestore.Visible = isDeleted && _isCurrentUserSuperAdmin;
            btnDelete.Text = isDeleted ? "🗑️ Hapus Permanen" : "🗑️ Hapus";

            // Protection untuk diri sendiri
            if (user.Id == _currentUserId)
            {
                btnDelete.Enabled = false; // Tidak bisa hapus diri sendiri
            }
        }

        private void ClearDetailForm()
        {
            txtEditUsername.Clear();
            txtEditEmail.Clear();
            txtEditName.Clear();
            txtEditPhone.Clear();
            chkEditIsActive.Checked = false;
            lblUserRoles.Text = "-";
            lblWarning.Visible = false;
            lblDeletedInfo.Visible = false;
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow?.DataBoundItem is not UserViewModel user)
            {
                MessageBox.Show("Pilih user yang akan diupdate!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validasi permission
            if (user.Roles.Contains("SuperAdmin") && !_isCurrentUserSuperAdmin)
            {
                MessageBox.Show("Anda tidak memiliki hak untuk mengubah SuperAdmin!",
                    "Akses Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // Konfirmasi jika menonaktifkan diri sendiri
            if (user.Id == _currentUserId && !chkEditIsActive.Checked && user.IsActive)
            {
                if (MessageBox.Show("Anda akan menonaktifkan akun Anda sendiri. Lanjutkan?",
                    "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
            }

            var updateData = new
            {
                fullName = txtEditName.Text,
                phoneNumber = txtEditPhone.Text,
                isActive = chkEditIsActive.Checked
            };

            try
            {
                ShowLoading(true);
                UpdateStatus("Updating user...");

                var json = JsonSerializer.Serialize(updateData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await ApiHelper.ExecuteWithRefreshAsync(client =>
                    client.PutAsync($"/api/v1/users/{user.Id}", content));

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ApiResponse<UserResponseDto>>(responseJson);

                    MessageBox.Show(result?.Message ?? "User berhasil diupdate", "Sukses",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await LoadUsersAsync(_currentPage);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(error);
                    MessageBox.Show($"Gagal update: {errorResponse?.Message ?? error}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowLoading(false);
                UpdateStatus("Ready");
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow?.DataBoundItem is not UserViewModel user)
                return;

            // Protection
            if (user.Roles.Contains("SuperAdmin"))
            {
                MessageBox.Show("SuperAdmin tidak dapat dihapus!", "Akses Ditolak",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (user.Id == _currentUserId)
            {
                MessageBox.Show("Anda tidak dapat menghapus diri sendiri!", "Akses Ditolak",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            var confirmMsg = user.IsDeleted
                ? $"Yakin hapus permanen user {user.Email}? Data tidak dapat dikembalikan!"
                : $"Yakin hapus user {user.Email}? User masih dapat direstore.";

            if (MessageBox.Show(confirmMsg, "Konfirmasi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                ShowLoading(true);
                UpdateStatus("Deleting user...");

                var response = await ApiHelper.ExecuteWithRefreshAsync(client =>
                    client.DeleteAsync($"/api/v1/users/{user.Id}"));

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ApiResponse<bool>>(json);

                    MessageBox.Show(result?.Message ?? "User berhasil dihapus", "Sukses",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    await LoadUsersAsync(_currentPage);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(error);
                    MessageBox.Show($"Gagal menghapus: {errorResponse?.Message ?? error}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowLoading(false);
                UpdateStatus("Ready");
            }
        }

        private async void btnRestore_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow?.DataBoundItem is not UserViewModel user || !user.IsDeleted)
                return;

            if (MessageBox.Show($"Yakin restore user {user.Email}?", "Konfirmasi",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                ShowLoading(true);
                UpdateStatus("Restoring user...");

                var response = await ApiHelper.ExecuteWithRefreshAsync(client =>
                    client.PostAsync($"/api/v1/users/{user.Id}/restore", null));

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("User berhasil direstore", "Sukses",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadUsersAsync(_currentPage);
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Gagal restore: {error}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowLoading(false);
                UpdateStatus("Ready");
            }
        }

        private async void btnAssignRole_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow?.DataBoundItem is not UserViewModel user)
                return;

            if (user.Roles.Contains("SuperAdmin"))
            {
                MessageBox.Show("Role SuperAdmin tidak dapat diubah!", "Akses Ditolak",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            using (var roleForm = new AssignRoleForm(user.Id, user.Roles))
            {
                if (roleForm.ShowDialog() == DialogResult.OK)
                {
                    await LoadUsersAsync(_currentPage);
                }
            }
        }

        private async void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (!_isCurrentUserSuperAdmin) return;

            if (dgvUsers.CurrentRow?.DataBoundItem is not UserViewModel user)
                return;

            using (var inputForm = new InputBoxForm($"Reset password untuk {user.Email}", "Password Baru:"))
            {
                if (inputForm.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(inputForm.InputValue))
                {
                    try
                    {
                        ShowLoading(true);
                        var resetData = new { userId = user.Id, newPassword = inputForm.InputValue };
                        var json = JsonSerializer.Serialize(resetData);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await ApiHelper.ExecuteWithRefreshAsync(client =>
                            client.PostAsync("/api/v1/users/reset-password", content));

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Password berhasil direset!", "Sukses",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            var error = await response.Content.ReadAsStringAsync();
                            MessageBox.Show($"Gagal reset password: {error}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        ShowLoading(false);
                    }
                }
            }
        }

        private async void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                var sfd = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = $"Users_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                };

                if (sfd.ShowDialog() != DialogResult.OK) return;

                ShowLoading(true);
                UpdateStatus("Exporting to Excel...");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Users");

                    // Headers
                    worksheet.Cell(1, 1).Value = "Username";
                    worksheet.Cell(1, 2).Value = "Email";
                    worksheet.Cell(1, 3).Value = "Full Name";
                    worksheet.Cell(1, 4).Value = "Phone";
                    worksheet.Cell(1, 5).Value = "Active";
                    worksheet.Cell(1, 6).Value = "Deleted";
                    worksheet.Cell(1, 7).Value = "Roles";
                    worksheet.Cell(1, 8).Value = "Branch";
                    worksheet.Cell(1, 9).Value = "Created At";

                    // Style header
                    var headerRange = worksheet.Range(1, 1, 1, 9);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

                    // Data
                    for (int i = 0; i < _users.Count; i++)
                    {
                        var user = _users[i];
                        worksheet.Cell(i + 2, 1).Value = user.Username;
                        worksheet.Cell(i + 2, 2).Value = user.Email;
                        worksheet.Cell(i + 2, 3).Value = user.FullName;
                        worksheet.Cell(i + 2, 4).Value = user.PhoneNumber;
                        worksheet.Cell(i + 2, 5).Value = user.IsActive ? "Yes" : "No";
                        worksheet.Cell(i + 2, 6).Value = user.IsDeleted ? "Yes" : "No";
                        worksheet.Cell(i + 2, 7).Value = string.Join(", ", user.Roles);
                        worksheet.Cell(i + 2, 8).Value = user.BranchName;
                        worksheet.Cell(i + 2, 9).Value = user.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                    }

                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(sfd.FileName);
                }

                MessageBox.Show("Export berhasil!", "Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error export: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowLoading(false);
                UpdateStatus("Ready");
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadUsersAsync(1);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Bisa implementasikan view detail form di sini
            }
        }

        #endregion

        #region Helper Methods

        private void HandleSessionExpired()
        {
            MessageBox.Show("Session telah expired. Silakan login kembali.", "Session Expired",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            ApiHelper.ClearSession();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ShowLoading(bool show)
        {
            progressBar.Visible = show;
            btnSearch.Enabled = !show;
            btnRefresh.Enabled = !show;
            btnEdit.Enabled = !show;
            btnDelete.Enabled = !show;
            Cursor = show ? System.Windows.Forms.Cursors.WaitCursor : System.Windows.Forms.Cursors.Default;
        }

        private void UpdateStatus(string message)
        {
            toolStripStatusLabel.Text = message;
            statusStrip.Refresh();
        }

        #endregion
    }

    #region Supporting Classes

    public class UserFilterModel
    {
        public int? BranchId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Search { get; set; }
        public string? Role { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
    }

    public class BranchViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class InputBoxForm : Form
    {
        private TextBox txtInput;
        private Button btnOk;
        private Button btnCancel;

        public string InputValue => txtInput.Text;

        public InputBoxForm(string title, string label)
        {
            this.Text = title;
            this.Size = new System.Drawing.Size(400, 180);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lbl = new Label
            {
                Text = label,
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(350, 20)
            };

            txtInput = new TextBox
            {
                Location = new System.Drawing.Point(20, 50),
                Size = new System.Drawing.Size(340, 23),
                UseSystemPasswordChar = true
            };

            btnOk = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new System.Drawing.Point(200, 90)
            };

            btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new System.Drawing.Point(285, 90)
            };

            this.Controls.Add(lbl);
            this.Controls.Add(txtInput);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);
            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }
    }

    #endregion
}