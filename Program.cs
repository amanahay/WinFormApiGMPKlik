using System;
using System.Windows.Forms;
using WinFormApiGMPKlik.Forms;
using WinFormApiGMPKlik.Properties;

namespace WinFormApiGMPKlik
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ✅ Pakai AppSettings.Instance — SATU sumber kebenaran
            // Ini memaksa LoadFromFile() dipanggil SEKALI di sini    private Settings _s => Settings.Default;
            var settings = Settings.Default;

            // Cek setting koneksi — pakai AppSettings, BUKAN Settings.Default
            if (string.IsNullOrEmpty(settings.ApiBaseUrl))
            {
                using var settingForm = new PengaturanKoneksi();
                if (settingForm.ShowDialog() != DialogResult.OK)
                    return;
            }

            // Loop login -> dashboard -> logout -> login lagi
            bool keepRunning = true;
            while (keepRunning)
            {
                using var loginForm = new LoginForm();
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    keepRunning = false;
                    break;
                }

                using var dashboard = new DashboardForm();
                var result = dashboard.ShowDialog();
                if (result != DialogResult.OK)
                {
                    keepRunning = false;
                }
            }
        }
    }
}