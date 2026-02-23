using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormApiGMPKlik.Properties;

namespace WinFormApiGMPKlik
{
    public partial class DaftarMandiri : Form
    {
        private readonly HttpClient _httpClient;

        public DaftarMandiri()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
        }

        private void DaftarMandiri_Load(object sender, EventArgs e)
        {
            // Cek apakah sudah ada setting koneksi
            if (string.IsNullOrEmpty(Settings.Default.ApiBaseUrl))
            {
                MessageBox.Show("Silakan atur koneksi server terlebih dahulu!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                using (var settingForm = new PengaturanKoneksi())
                {
                    if (settingForm.ShowDialog() != DialogResult.OK)
                    {
                        this.Close();
                        return;
                    }
                }
            }

            _httpClient.BaseAddress = new Uri(Settings.Default.ApiBaseUrl);

            // Set default state
            numBranchId.Enabled = true;
            chkTamu.Checked = false;
            lblStatus.Text = "Status: Siap";
        }

        private void chkTamu_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTamu.Checked)
            {
                numBranchId.Value = 0;
                numBranchId.Enabled = false;
            }
            else
            {
                numBranchId.Enabled = true;
            }
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            // Validasi input
            if (!ValidasiInput())
                return;

            btnRegister.Enabled = false;
            lblStatus.Text = "Status: Mendaftarkan...";
            lblStatus.ForeColor = System.Drawing.Color.Blue;

            try
            {
                // Siapkan data
                var registerData = new
                {
                    email = txtEmail.Text.Trim(),
                    password = txtPassword.Text,
                    fullName = txtFullName.Text.Trim(),
                    phoneNumber = txtPhone.Text.Trim(),
                    branchId = chkTamu.Checked ? (int?)null : (int?)numBranchId.Value,
                    roles = (List<string>)null! // Biar server yang tentukan otomatis
                };

                var json = JsonSerializer.Serialize(registerData, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Kirim request
                var response = await ApiHelper.ExecuteWithRefreshAsync(client =>
              client.PostAsync("/api/v1/users/register", content));
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    lblStatus.Text = "Status: Pendaftaran berhasil!";
                    lblStatus.ForeColor = System.Drawing.Color.Green;

                    MessageBox.Show("Pendaftaran berhasil! Silakan login.", "Sukses",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear form atau close
                    ClearForm();
                }
                else
                {
                    lblStatus.Text = $"Status: Gagal ({(int)response.StatusCode})";
                    lblStatus.ForeColor = System.Drawing.Color.Red;

                    // Parse error message dari ApiResponse
                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<ApiErrorResponse>(responseString);
                        var errorMsg = errorResponse?.Message ?? "Terjadi kesalahan saat mendaftar";
                        var errorDetails = errorResponse?.Errors != null
                            ? string.Join("\n", errorResponse.Errors)
                            : "";

                        MessageBox.Show($"{errorMsg}\n{errorDetails}", "Gagal Mendaftar",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch
                    {
                        MessageBox.Show($"Error: {response.StatusCode}\n{responseString}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Session expired. Silakan login kembali.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
            finally
            {
                btnRegister.Enabled = true;
            }
        }

        private bool ValidasiInput()
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email tidak boleh kosong!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password tidak boleh kosong!", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Password dan konfirmasi password tidak cocok!", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return false;
            }

            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Password minimal 6 karakter!", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            if (!chkTamu.Checked && numBranchId.Value == 0)
            {
                MessageBox.Show("Pilih Branch ID atau centang 'Daftar sebagai Tamu'!", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtEmail.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            txtFullName.Clear();
            txtPhone.Clear();
            numBranchId.Value = 0;
            chkTamu.Checked = false;
            txtEmail.Focus();
        }

        // Class untuk deserialize error response
        public class ApiErrorResponse
        {
            [JsonPropertyName("message")]
            public string? Message { get; set; }

            [JsonPropertyName("errors")]
            public List<ErrorDetail>? Errors { get; set; }
        }

        public class ErrorDetail
        {
            [JsonPropertyName("message")]
            public string? Message { get; set; }

            [JsonPropertyName("code")]
            public string? Code { get; set; }
        }
    }
}