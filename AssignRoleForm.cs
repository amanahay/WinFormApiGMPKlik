using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using WinFormApiGMPKlik.Properties;

namespace WinFormApiGMPKlik.Forms
{
    public partial class AssignRoleForm : Form
    {
        private readonly string _userId;
        private readonly List<string> _currentRoles;
        private readonly HttpClient _httpClient;

        public AssignRoleForm(string userId, List<string> currentRoles)
        {
            InitializeComponent();
            _userId = userId;
            _currentRoles = currentRoles;
            _httpClient = new HttpClient { BaseAddress = new Uri(Settings.Default.ApiBaseUrl) };
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Settings.Default.AccessToken);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private async void AssignRoleForm_Load(object sender, EventArgs e)
        {
            await LoadAvailableRoles();

            // Cek current roles
            foreach (var role in _currentRoles)
            {
                var item = clbRoles.Items.Cast<RoleItem>().FirstOrDefault(r => r.Name == role);
                if (item != null)
                {
                    clbRoles.SetItemChecked(clbRoles.Items.IndexOf(item), true);
                }
            }

            // Proteksi: Jika user adalah SuperAdmin, semua checkbox disable
            if (_currentRoles.Contains("SuperAdmin"))
            {
                lblWarning.Text = "SuperAdmin roles are immutable";
                lblWarning.Visible = true;
                btnSave.Enabled = false;
                for (int i = 0; i < clbRoles.Items.Count; i++)
                    clbRoles.SetItemCheckState(i, CheckState.Indeterminate);
            }
        }

        private async Task LoadAvailableRoles()
        {
            try
            {
                var response = await ApiHelper.ExecuteWithRefreshAsync(client =>
                    client.GetAsync("/api/v1/users/roles"));

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<ApiResponse<List<string>>>(json);

                    clbRoles.Items.Clear();
                    foreach (var role in result!.Data!)
                    {
                        if (role == "SuperAdmin" && !IsCurrentUserSuperAdmin())
                            continue;

                        clbRoles.Items.Add(new RoleItem { Name = role }, false);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Session expired. Silakan login kembali.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal load roles: {ex.Message}");
            }
        }


        private bool IsCurrentUserSuperAdmin()
        {
            // Cek dari token atau context
            // Simplified: cek dari settings atau static variable
            return true; // Implementasi sesuai kebutuhan
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            var selectedRoles = clbRoles.CheckedItems.Cast<RoleItem>().Select(r => r.Name).ToList();

            // Proteksi: Tidak boleh assign SuperAdmin kecuali memang SuperAdmin
            if (selectedRoles.Contains("SuperAdmin") && !IsCurrentUserSuperAdmin())
            {
                MessageBox.Show("Anda tidak dapat memberikan role SuperAdmin", "Akses Ditolak",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            var assignData = new
            {
                userId = _userId,
                roles = selectedRoles,
                removeExisting = true
            };

            try
            {
                var json = JsonSerializer.Serialize(assignData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await ApiHelper.ExecuteWithRefreshAsync(client =>
                    client.PostAsync("/api/v1/users/assign-roles", content));

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Roles berhasil diupdate");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Gagal: {error}");
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Session expired.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }

    public class RoleItem
    {
        public string Name { get; set; }
        public override string ToString() => Name;
    }
}