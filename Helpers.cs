using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WinFormApiGMPKlik.Properties;

namespace WinFormApiGMPKlik
{
    public static class ApiHelper
    {
        private static Settings _s => Settings.Default;

        // ─────────────────────────────────────────────────────────────
        // GetAuthorizedClient
        // TIDAK memanggil Reload() — baca langsung dari memory Settings
        // supaya token yang baru disimpan tidak tertimpa oleh Reload()
        // ─────────────────────────────────────────────────────────────
        public static HttpClient GetAuthorizedClient()
        {
            var token = Settings.Default.AccessToken?.Trim();
            var baseUrl = Settings.Default.ApiBaseUrl?.Trim();

            System.Diagnostics.Debug.WriteLine(
                $"[GetAuthorizedClient] token={token?.Substring(0, Math.Min(20, token?.Length ?? 0))}...");

            if (string.IsNullOrEmpty(baseUrl))
                throw new InvalidOperationException("ApiBaseUrl tidak di-setting!");

            if (string.IsNullOrWhiteSpace(token) || token == "AccessToken")
                throw new UnauthorizedAccessException("Token tidak ditemukan. Silakan login ulang.");

            if (!VerifyTokenFormat(token))
                throw new UnauthorizedAccessException("Format token tidak valid.");

            var client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl.TrimEnd('/')),
                Timeout = TimeSpan.FromSeconds(30)
            };

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return client;
        }

        // ─────────────────────────────────────────────────────────────
        // ExecuteWithRefreshAsync
        // Langsung pakai token dari memory — biar SERVER jawab 401
        // kalau token tidak valid, bukan kita yang nebak lokal.
        // ─────────────────────────────────────────────────────────────
        public static async Task<HttpResponseMessage> ExecuteWithRefreshAsync(
            Func<HttpClient, Task<HttpResponseMessage>> apiCall)
        {
            // Langsung pakai token yang ada di memory
            using var client = GetAuthorizedClient();
            var response = await apiCall(client);

            System.Diagnostics.Debug.WriteLine(
                $"[ExecuteWithRefreshAsync] Response: {response.StatusCode}");

            // Hanya kalau server jawab 401 → coba refresh sekali
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                System.Diagnostics.Debug.WriteLine("[ExecuteWithRefreshAsync] 401 → mencoba refresh token...");

                var refreshed = await RefreshTokenAsync();
                if (refreshed)
                {
                    using var newClient = GetAuthorizedClient();
                    response = await apiCall(newClient);
                }
                else
                {
                    throw new UnauthorizedAccessException("Session tidak valid. Silakan login kembali.");
                }
            }

            return response;
        }

        // ─────────────────────────────────────────────────────────────
        // RefreshTokenAsync — hanya dipanggil kalau server jawab 401
        // ─────────────────────────────────────────────────────────────
        public static async Task<bool> RefreshTokenAsync()
        {
            try
            {
                var refreshToken = Settings.Default.RefreshToken;
                if (string.IsNullOrEmpty(refreshToken))
                    return false;

                using var client = new HttpClient
                {
                    BaseAddress = new Uri(Settings.Default.ApiBaseUrl.TrimEnd('/')),
                    Timeout = TimeSpan.FromSeconds(30)
                };

                var json = JsonSerializer.Serialize(new { refreshToken });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/api/v1/users/refresh-token", content);

                if (!response.IsSuccessStatusCode)
                    return false;

                var body = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse<LoginResponseDto>>(body,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result?.Data == null || string.IsNullOrEmpty(result.Data.Token))
                    return false;

                Settings.Default.AccessToken = result.Data.Token;
                Settings.Default.RefreshToken = result.Data.RefreshToken ?? string.Empty;
                Settings.Default.TokenExpiry = result.Data.ExpiresAt.ToString("O");

                if (result.Data.User != null)
                {
                    Settings.Default.CurrentUserId = result.Data.User.Id;
                    Settings.Default.CurrentUserRoles = string.Join(",", result.Data.User.Roles);
                    Settings.Default.CurrentUserEmail = result.Data.User.Email;
                }

                Settings.Default.Save();
                System.Diagnostics.Debug.WriteLine("[RefreshTokenAsync] Token berhasil diperbarui.");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[RefreshTokenAsync] Gagal: {ex.Message}");
                return false;
            }
        }

        // ─────────────────────────────────────────────────────────────
        // IsTokenExpired — hanya untuk DashboardForm countdown timer
        // ─────────────────────────────────────────────────────────────
        public static bool IsTokenExpired()
        {
            var expiryStr = Settings.Default.TokenExpiry;
            if (string.IsNullOrEmpty(expiryStr)) return true;

            if (DateTime.TryParse(expiryStr, null,
                System.Globalization.DateTimeStyles.RoundtripKind, out DateTime expiry))
            {
                var isExpired = DateTime.UtcNow >= expiry.ToUniversalTime().AddMinutes(-2);
                System.Diagnostics.Debug.WriteLine(
                    $"[IsTokenExpired] Expiry={expiry.ToUniversalTime():HH:mm:ss} UTC | " +
                    $"Now={DateTime.UtcNow:HH:mm:ss} UTC | Expired={isExpired}");
                return isExpired;
            }
            return true;
        }

        // ─────────────────────────────────────────────────────────────
        // EnsureSettingsLoaded
        // HANYA dipanggil saat pertama buka form dari luar (misal Program.cs)
        // JANGAN panggil ini setelah Login menyimpan token!
        // ─────────────────────────────────────────────────────────────
        public static void EnsureSettingsLoaded()
        {
            try
            {
                Settings.Default.Reload();
                var token = Settings.Default.AccessToken;
                System.Diagnostics.Debug.WriteLine(
                    $"[EnsureSettingsLoaded] Token: {(string.IsNullOrEmpty(token) ? "EMPTY" : token.Substring(0, Math.Min(20, token.Length)) + "...")}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[EnsureSettingsLoaded] Error: {ex.Message}");
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Helpers lainnya
        // ─────────────────────────────────────────────────────────────
        public static bool VerifyTokenFormat(string token)
        {
            if (string.IsNullOrEmpty(token) || token == "AccessToken") return false;
            return token.Split('.').Length == 3;
        }

        public static async Task<bool> CheckInternetConnectionAsync()
        {
            try
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
                var tasks = new List<Task<bool>>
                {
                    CheckConnectionAsync(client, "https://www.google.com"),
                    CheckConnectionAsync(client, "https://www.cloudflare.com"),
                    CheckConnectionAsync(client, _s.ApiBaseUrl)
                };
                var results = await Task.WhenAll(tasks);
                return results.Any(r => r);
            }
            catch { return false; }
        }

        public static async Task<bool> CheckConnectionAsync(HttpClient client, string url)
        {
            try
            {
                var r = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                return r.IsSuccessStatusCode || r.StatusCode == System.Net.HttpStatusCode.Unauthorized;
            }
            catch { return false; }
        }

        public static void ClearSession()
        {
            Settings.Default.AccessToken = string.Empty;
            Settings.Default.RefreshToken = string.Empty;
            Settings.Default.CurrentUserId = string.Empty;
            Settings.Default.CurrentUserRoles = string.Empty;
            Settings.Default.TokenExpiry = string.Empty;
            Settings.Default.Save();
        }

        public static string EncryptPassword(string plain)
        {
            if (string.IsNullOrEmpty(plain)) return string.Empty;
            try
            {
                return Convert.ToBase64String(
                    ProtectedData.Protect(Encoding.UTF8.GetBytes(plain), null, DataProtectionScope.CurrentUser));
            }
            catch { return string.Empty; }
        }

        public static string DecryptPassword(string encrypted)
        {
            if (string.IsNullOrEmpty(encrypted)) return string.Empty;
            try
            {
                return Encoding.UTF8.GetString(
                    ProtectedData.Unprotect(Convert.FromBase64String(encrypted), null, DataProtectionScope.CurrentUser));
            }
            catch { return string.Empty; }
        }
    }

    // ═══════════════════════════════════════════════════════
    // DTOs & ViewModels
    // ═══════════════════════════════════════════════════════

    public class PaginationMetadata
    {
        [JsonPropertyName("page")] public int Page { get; set; }
        [JsonPropertyName("pageSize")] public int PageSize { get; set; }
        [JsonPropertyName("totalCount")] public int TotalCount { get; set; }
        [JsonPropertyName("totalPages")] public int TotalPages { get; set; }
        [JsonPropertyName("hasNext")] public bool HasNext { get; set; }
        [JsonPropertyName("hasPrevious")] public bool HasPrevious { get; set; }
    }

    public class ApiResponse<T>
    {
        [JsonPropertyName("success")] public bool IsSuccess { get; set; }
        [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;
        [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;
        [JsonPropertyName("title")] public string Title { get; set; } = string.Empty;
        [JsonPropertyName("data")] public T? Data { get; set; }
        [JsonPropertyName("metadata")] public PaginationMetadata? Metadata { get; set; }
        [JsonPropertyName("timestamp")] public DateTime Timestamp { get; set; }
        [JsonPropertyName("statusCode")] public int StatusCode { get; set; }
        [JsonPropertyName("requestId")] public string RequestId { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        [JsonPropertyName("token")] public string Token { get; set; } = string.Empty;
        [JsonPropertyName("refreshToken")] public string RefreshToken { get; set; } = string.Empty;
        [JsonPropertyName("expiresAt")] public DateTime ExpiresAt { get; set; }
        [JsonPropertyName("user")] public UserResponseDto? User { get; set; }
    }

    public class UserResponseDto
    {
        [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
        [JsonPropertyName("username")] public string Username { get; set; } = string.Empty;
        [JsonPropertyName("email")] public string Email { get; set; } = string.Empty;
        [JsonPropertyName("fullName")] public string? FullName { get; set; }
        [JsonPropertyName("phoneNumber")] public string? PhoneNumber { get; set; }
        [JsonPropertyName("isActive")] public bool IsActive { get; set; }
        [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
        [JsonPropertyName("roles")] public List<string> Roles { get; set; } = new();
    }

    public class UserViewModel
    {
        [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
        [JsonPropertyName("username")] public string Username { get; set; } = string.Empty;
        [JsonPropertyName("email")] public string Email { get; set; } = string.Empty;
        [JsonPropertyName("fullName")] public string? FullName { get; set; }
        [JsonPropertyName("phoneNumber")] public string? PhoneNumber { get; set; }
        [JsonPropertyName("avatar")] public string? Avatar { get; set; }
        [JsonPropertyName("isActive")] public bool IsActive { get; set; }
        [JsonPropertyName("isDeleted")] public bool IsDeleted { get; set; }
        [JsonPropertyName("deletedAt")] public DateTime? DeletedAt { get; set; }
        [JsonPropertyName("deletedBy")] public string? DeletedBy { get; set; }
        [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
        [JsonPropertyName("modifiedAt")] public DateTime? ModifiedAt { get; set; }
        [JsonPropertyName("roles")] public List<string> Roles { get; set; } = new();
        [JsonPropertyName("branchId")] public int? BranchId { get; set; }
        [JsonPropertyName("branchName")] public string? BranchName { get; set; }
        [JsonPropertyName("profile")] public UserProfileViewModel? Profile { get; set; }
    }

    public class UserProfileViewModel
    {
        [JsonPropertyName("about")] public string? About { get; set; }
        [JsonPropertyName("gender")] public string? Gender { get; set; }
        [JsonPropertyName("birthDate")] public DateTime? BirthDate { get; set; }
        [JsonPropertyName("address")] public string? Address { get; set; }
        [JsonPropertyName("city")] public string? City { get; set; }
        [JsonPropertyName("country")] public string? Country { get; set; }
        [JsonPropertyName("lastLoginAt")] public DateTime? LastLoginAt { get; set; }
    }
}