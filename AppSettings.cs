using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace WinFormApiGMPKlik
{
    /// <summary>
    /// Singleton settings yang disimpan ke JSON di LocalAppData.
    /// Lebih reliable daripada Settings.settings di .NET Core.
    /// </summary>
    public class AppSettings
    {
        // Path: C:\Users\<user>\AppData\Local\GMPKlik\settings.json
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "GMPKlik", "settings.json");

        // ── Properties ──────────────────────────────────────────────
        public string ApiBaseUrl { get; set; } = "https://localhost:7170";
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string CurrentUserId { get; set; } = string.Empty;
        public string CurrentUserRoles { get; set; } = string.Empty;
        public string TokenExpiry { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
        public string SavedEmail { get; set; } = string.Empty;
        public string SavedPassword { get; set; } = string.Empty; // DPAPI encrypted

        // ── Singleton ───────────────────────────────────────────────
        private static AppSettings? _instance;
        private static readonly object _lock = new();

        public static AppSettings Instance
        {
            get
            {
                if (_instance == null)
                    lock (_lock)
                        _instance ??= LoadFromFile();
                return _instance;
            }
        }

        // ── Load ────────────────────────────────────────────────────
        private static AppSettings LoadFromFile()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    var json = File.ReadAllText(SettingsPath);
                    var obj = JsonSerializer.Deserialize<AppSettings>(json);
                    if (obj != null) return obj;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AppSettings.Load] {ex.Message}");
            }
            return new AppSettings();
        }

        // ── Save ────────────────────────────────────────────────────
        public void Save()
        {
            try
            {
                var dir = Path.GetDirectoryName(SettingsPath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsPath, json);

                System.Diagnostics.Debug.WriteLine($"[AppSettings.Save] OK → {SettingsPath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AppSettings.Save] ERROR: {ex.Message}");
                System.Windows.Forms.MessageBox.Show(
                    $"Gagal menyimpan settings:\n{ex.Message}", "Warning",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        // ── Password Encrypt/Decrypt (DPAPI - Windows built-in) ─────
        public void SetPassword(string plain)
        {
            if (string.IsNullOrEmpty(plain)) { SavedPassword = string.Empty; return; }
            try
            {
                var encrypted = ProtectedData.Protect(
                    Encoding.UTF8.GetBytes(plain), null, DataProtectionScope.CurrentUser);
                SavedPassword = Convert.ToBase64String(encrypted);
            }
            catch { SavedPassword = string.Empty; }
        }

        public string GetPassword()
        {
            if (string.IsNullOrEmpty(SavedPassword)) return string.Empty;
            try
            {
                var decrypted = ProtectedData.Unprotect(
                    Convert.FromBase64String(SavedPassword), null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decrypted);
            }
            catch { return string.Empty; }
        }
    }
}