using System.Net.Http;
using System.Text;
using System.Text.Json;
using EasyPeasyAPP.Models;

namespace EasyPeasyAPP.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private const string FIREBASE_API_KEY = "***REMOVED***";
        private const string AUTH_URL = "https://identitytoolkit.googleapis.com/v1/accounts";

        // SecureStorage keys
        private const string TOKEN_KEY = "auth_token";
        private const string USER_ID_KEY = "user_id";
        private const string USER_EMAIL_KEY = "user_email";
        private const string USER_DISPLAY_NAME_KEY = "user_display_name";

        private string? _currentUserId;
        private string? _idToken;
        private User? _currentUser;

        public AuthService()
        {
            _httpClient = new HttpClient();
            // Učitaj token iz storage-a pri inicijalizaciji
            LoadAuthFromStorage();
        }

        public bool IsAuthenticated => !string.IsNullOrEmpty(_idToken);

        public async Task<User> LoginWithEmailAsync(string email, string password)
        {
            try
            {
                var requestData = new
                {
                    email = email,
                    password = password,
                    returnSecureToken = true
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestData),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                    $"{AUTH_URL}:signInWithPassword?key={FIREBASE_API_KEY}",
                    content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    var errorObj = JsonSerializer.Deserialize<FirebaseErrorResponse>(error);
                    throw new Exception(errorObj?.error?.message ?? "Login failed");
                }

                var result = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<FirebaseAuthResponse>(result);

                if (authResponse == null)
                    throw new Exception("Invalid response from server");

                _idToken = authResponse.idToken;
                _currentUserId = authResponse.localId;

                _currentUser = new User
                {
                    Id = authResponse.localId ?? string.Empty,
                    Email = authResponse.email ?? email,
                    DisplayName = authResponse.displayName ?? email.Split('@')[0],
                    CreatedAt = DateTime.UtcNow
                };

                // Sačuvaj u SecureStorage
                await SaveAuthToStorage();

                return _currentUser;
            }
            catch (Exception ex)
            {
                throw new Exception($"Login failed: {ex.Message}");
            }
        }

        public async Task<User> RegisterWithEmailAsync(string email, string password, string displayName)
        {
            try
            {
                var requestData = new
                {
                    email = email,
                    password = password,
                    returnSecureToken = true
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestData),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                    $"{AUTH_URL}:signUp?key={FIREBASE_API_KEY}",
                    content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    var errorObj = JsonSerializer.Deserialize<FirebaseErrorResponse>(error);
                    throw new Exception(errorObj?.error?.message ?? "Registration failed");
                }

                var result = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<FirebaseAuthResponse>(result);

                if (authResponse == null)
                    throw new Exception("Invalid response from server");

                _idToken = authResponse.idToken;
                _currentUserId = authResponse.localId;

                await UpdateProfileAsync(displayName);

                _currentUser = new User
                {
                    Id = authResponse.localId ?? string.Empty,
                    Email = authResponse.email ?? email,
                    DisplayName = displayName,
                    CreatedAt = DateTime.UtcNow
                };

                // Sačuvaj u SecureStorage
                await SaveAuthToStorage();

                return _currentUser;
            }
            catch (Exception ex)
            {
                throw new Exception($"Registration failed: {ex.Message}");
            }
        }

        public async Task LogoutAsync()
        {
            _idToken = null;
            _currentUserId = null;
            _currentUser = null;

            // Obriši iz SecureStorage
            SecureStorage.Remove(TOKEN_KEY);
            SecureStorage.Remove(USER_ID_KEY);
            SecureStorage.Remove(USER_EMAIL_KEY);
            SecureStorage.Remove(USER_DISPLAY_NAME_KEY);

            await Task.CompletedTask;
        }

        public Task<User?> GetCurrentUserAsync()
        {
            return Task.FromResult(_currentUser);
        }

        private async Task UpdateProfileAsync(string displayName)
        {
            try
            {
                var requestData = new
                {
                    idToken = _idToken,
                    displayName = displayName,
                    returnSecureToken = false
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestData),
                    Encoding.UTF8,
                    "application/json");

                await _httpClient.PostAsync(
                    $"{AUTH_URL}:update?key={FIREBASE_API_KEY}",
                    content);
            }
            catch
            {
                // Ignore profile update errors
            }
        }

        private async Task SaveAuthToStorage()
        {
            try
            {
                if (!string.IsNullOrEmpty(_idToken))
                    await SecureStorage.SetAsync(TOKEN_KEY, _idToken);

                if (!string.IsNullOrEmpty(_currentUserId))
                    await SecureStorage.SetAsync(USER_ID_KEY, _currentUserId);

                if (!string.IsNullOrEmpty(_currentUser?.Email))
                    await SecureStorage.SetAsync(USER_EMAIL_KEY, _currentUser.Email);

                if (!string.IsNullOrEmpty(_currentUser?.DisplayName))
                    await SecureStorage.SetAsync(USER_DISPLAY_NAME_KEY, _currentUser.DisplayName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save auth: {ex.Message}");
            }
        }

        private void LoadAuthFromStorage()
        {
            try
            {
                _idToken = SecureStorage.GetAsync(TOKEN_KEY).Result;
                _currentUserId = SecureStorage.GetAsync(USER_ID_KEY).Result;

                var email = SecureStorage.GetAsync(USER_EMAIL_KEY).Result;
                var displayName = SecureStorage.GetAsync(USER_DISPLAY_NAME_KEY).Result;

                if (!string.IsNullOrEmpty(_idToken) && !string.IsNullOrEmpty(_currentUserId))
                {
                    _currentUser = new User
                    {
                        Id = _currentUserId,
                        Email = email ?? string.Empty,
                        DisplayName = displayName ?? string.Empty,
                        CreatedAt = DateTime.UtcNow
                    };
                }
            }
            catch
            {
                // Ako nema sačuvanih podataka, ostavi null
                _idToken = null;
                _currentUserId = null;
                _currentUser = null;
            }
        }

        private class FirebaseAuthResponse
        {
            public string? idToken { get; set; }
            public string? email { get; set; }
            public string? refreshToken { get; set; }
            public string? expiresIn { get; set; }
            public string? localId { get; set; }
            public string? displayName { get; set; }
        }

        private class FirebaseErrorResponse
        {
            public ErrorDetail? error { get; set; }
        }

        private class ErrorDetail
        {
            public string? message { get; set; }
        }
    }
}