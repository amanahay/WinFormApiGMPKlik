using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Extensions.Logging;
using System.Text;

namespace WinFormApiGMPKlik.Services
{
    /// <summary>
    /// Service untuk manage user data via API dengan Bearer auth
    /// Mengikuti SOLID: Single Responsibility
    /// </summary>
    public interface IUserService
    {
        Task<(List<UserViewModel> Users, PaginationMetadata Metadata)> GetUsersAsync(UserQueryParams query);
        Task<UserViewModel> GetUserByIdAsync(string userId);
        Task<bool> UpdateUserAsync(string userId, UpdateUserRequest request);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> RestoreUserAsync(string userId);
    }

    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<UserService> _logger;

        public UserService(IHttpClientFactory httpClientFactory, ILogger<UserService> logger)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<(List<UserViewModel> Users, PaginationMetadata Metadata)> GetUsersAsync(UserQueryParams query)
        {
            try
            {
                var queryString = BuildQueryString(query);
                var url = $"/api/v1/users?{queryString}";

                var response = await ApiHelper.ExecuteWithRefreshAsync(client =>
                    client.GetAsync(url));

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogWarning("Unauthorized access - token refresh failed");
                    throw new UnauthorizedAccessException("Session expired or invalid token");
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"API error: {response.StatusCode} - {errorContent}");
                    throw new HttpRequestException($"Failed to fetch users: {response.StatusCode}");
                }

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse<List<UserViewModel>>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return (result?.Data! ?? new(), result?.Metadata!);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching users: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<UserViewModel> GetUserByIdAsync(string userId)
        {
            var response = await ApiHelper.ExecuteWithRefreshAsync(client =>
                client.GetAsync($"/api/v1/users/{userId}"));

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<UserViewModel>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result?.Data!;
        }

        public async Task<bool> UpdateUserAsync(string userId, UpdateUserRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await ApiHelper.ExecuteWithRefreshAsync(client =>
                client.PutAsync($"/api/v1/users/{userId}", content));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var response = await ApiHelper.ExecuteWithRefreshAsync(client =>
                client.DeleteAsync($"/api/v1/users/{userId}"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RestoreUserAsync(string userId)
        {
            var response = await ApiHelper.ExecuteWithRefreshAsync(client =>
                client.PostAsync($"/api/v1/users/{userId}/restore", null));

            return response.IsSuccessStatusCode;
        }

        private string BuildQueryString(UserQueryParams query)
        {
            var parameters = new List<string>
            {
                $"page={query.Page}",
                $"pageSize={query.PageSize}",
                $"sortBy={query.SortBy}",
                $"sortDescending={query.SortDescending}"
            };

            if (!string.IsNullOrEmpty(query.Search))
                parameters.Add($"search={Uri.EscapeDataString(query.Search)}");

            if (query.IsActive.HasValue)
                parameters.Add($"isActive={query.IsActive.Value}");

            if (query.IsDeleted.HasValue)
                parameters.Add($"isDeleted={query.IsDeleted.Value}");

            if (!string.IsNullOrEmpty(query.Role))
                parameters.Add($"role={query.Role}");

            if (query.CreatedFrom.HasValue)
                parameters.Add($"createdFrom={query.CreatedFrom:yyyy-MM-dd}");

            if (query.CreatedTo.HasValue)
                parameters.Add($"createdTo={query.CreatedTo:yyyy-MM-dd}");

            return string.Join("&", parameters);
        }
    }

    // DTOs
    public class UserQueryParams
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Role { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public string SortBy { get; set; } = "CreatedAt";
        public bool SortDescending { get; set; } = true;
    }

    public class UpdateUserRequest
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}