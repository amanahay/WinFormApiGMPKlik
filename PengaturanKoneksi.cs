using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormApiGMPKlik.Properties;

namespace WinFormApiGMPKlik
{
    public partial class PengaturanKoneksi : Form
    {
        public PengaturanKoneksi()
        {
            InitializeComponent();
        }

        private void PengaturanKoneksi_Load(object sender, EventArgs e)
        {
            // Load existing settings
            txtApiUrl.Text = Settings.Default.ApiBaseUrl;
            lblStatus.Text = "Status: Siap";
        }

        private async void btnTes_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtApiUrl.Text))
            {
                MessageBox.Show("Alamat server tidak boleh kosong!", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnTes.Enabled = false;
            lblStatus.Text = "Status: Mengetes koneksi...";

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(txtApiUrl.Text);
                    client.Timeout = TimeSpan.FromSeconds(10);

                    // Tes endpoint health atau version
                    var response = await client.GetAsync("/api/v1/users/register");

                    // Meskipun 404 atau 405, artinya server nyambung
                    if (response.IsSuccessStatusCode ||
                        (int)response.StatusCode == 404 ||
                        (int)response.StatusCode == 405)
                    {
                        lblStatus.Text = "Status: Koneksi berhasil!";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                        MessageBox.Show("Koneksi ke server berhasil!", "Sukses",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        lblStatus.Text = $"Status: Error {(int)response.StatusCode}";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        MessageBox.Show("Server merespons tapi ada masalah.", "Peringatan",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Status: Gagal terhubung";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show($"Gagal terhubung ke server:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnTes.Enabled = true;
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtApiUrl.Text))
            {
                MessageBox.Show("Alamat server tidak boleh kosong!", "Validasi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Validasi URL format
                var uri = new Uri(txtApiUrl.Text);

                // Save to settings
                Settings.Default.ApiBaseUrl = txtApiUrl.Text;
                Settings.Default.Save();

                MessageBox.Show("Pengaturan berhasil disimpan!", "Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (UriFormatException)
            {
                MessageBox.Show("Format URL tidak valid!\nContoh: http://localhost:5000",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}