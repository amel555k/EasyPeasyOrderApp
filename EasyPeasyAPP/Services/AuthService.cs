using System.Net.Http;
using System.Text;
using System.Text.Json;
using EasyPeasyAPP.Models;

namespace EasyPeasyAPP.Services
{
    public class AuthService : IAuthService
    {
        private const string FIREBASE_API_KEY = "***REMOVED***";
        private const string FIREBASE_DB_URL = "***REMOVED***";

        private readonly HttpClient _httpClient;
        private string? _idToken;
        private UserModel? _currentUser;

        public AuthService()
        {
            _httpClient = new HttpClient();
        }

        public bool IsAuthenticated => !string.IsNullOrEmpty(_idToken);
        public UserModel? CurrentUser => _currentUser;

        #region Initialization
        public async Task InitializeAsync()
        {
            await LoadFromStorageAsync();

            if (!string.IsNullOrEmpty(_idToken))
            {
                try
                {
                    _currentUser = await GetCurrentUserAsync();
                }
                catch
                {
                    _idToken = null;
                    _currentUser = null;
                }
            }
        }

        private async Task LoadFromStorageAsync()
        {
            try
            {
                _idToken = await SecureStorage.Default.GetAsync("auth_token");

                // Također učitaj user_uid
                var uid = await SecureStorage.Default.GetAsync("user_uid");

                // Ako imaš i token i uid, dohvati korisnika odmah
                if (!string.IsNullOrEmpty(_idToken) && !string.IsNullOrEmpty(uid))
                {
                    try
                    {
                        var dbResponse = await _httpClient.GetAsync($"{FIREBASE_DB_URL}korisnici/{uid}.json?auth={_idToken}");
                        dbResponse.EnsureSuccessStatusCode();
                        var userJson = await dbResponse.Content.ReadAsStringAsync();

                        _currentUser = JsonSerializer.Deserialize<UserModel>(userJson)!;
                        _currentUser.Uid = uid;
                    }
                    catch
                    {
                        // Ako ne može dohvatiti, obriši token
                        _idToken = null;
                        _currentUser = null;
                    }
                }
            }
            catch
            {
                _idToken = null;
                _currentUser = null;
            }
        }

        private async Task SaveToStorageAsync()
        {
            if (!string.IsNullOrEmpty(_idToken))
                await SecureStorage.Default.SetAsync("auth_token", _idToken);
        }
        #endregion

        #region Register
        public async Task<UserModel> RegisterWithEmailAsync(string email, string password, string ime, string? telefon = null, string? adresa = null)
        {
            var requestData = new
            {
                email,
                password,
                returnSecureToken = true
            };
            var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={FIREBASE_API_KEY}", content);
            response.EnsureSuccessStatusCode();

            var resultJson = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<FirebaseAuthResponse>(resultJson)!;

            _idToken = authResponse.idToken;
            string uid = authResponse.localId!;

            var newUser = new UserModel
            {
                Uid = uid,
                Email = email,
                Ime = ime,
                Uloga = "kupac",
                Telefon = telefon,
                Adresa = adresa
            };

            var dbContent = new StringContent(JsonSerializer.Serialize(newUser), Encoding.UTF8, "application/json");
            var dbResponse = await _httpClient.PutAsync($"{FIREBASE_DB_URL}korisnici/{uid}.json?auth={_idToken}", dbContent);
            dbResponse.EnsureSuccessStatusCode();

            _currentUser = newUser;
            await SaveToStorageAsync();
            await SecureStorage.Default.SetAsync("user_uid", uid); // ← Dodato
            return _currentUser;
        }
        #endregion

        #region Login
        public async Task<UserModel> LoginWithEmailAsync(string email, string password)
        {
            var requestData = new
            {
                email,
                password,
                returnSecureToken = true
            };
            var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={FIREBASE_API_KEY}", content);
            response.EnsureSuccessStatusCode();

            var resultJson = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<FirebaseAuthResponse>(resultJson)!;

            _idToken = authResponse.idToken;
            string uid = authResponse.localId!;

            // Dohvati user podatke iz DB
            var dbResponse = await _httpClient.GetAsync($"{FIREBASE_DB_URL}korisnici/{uid}.json?auth={_idToken}");
            dbResponse.EnsureSuccessStatusCode();
            var userJson = await dbResponse.Content.ReadAsStringAsync();

            _currentUser = JsonSerializer.Deserialize<UserModel>(userJson)!;
            _currentUser.Uid = uid;

            await SaveToStorageAsync();
            await SecureStorage.Default.SetAsync("user_uid", uid); // ← Već imaš ovo

            return _currentUser;
        }
        #endregion

        #region CurrentUser
        public async Task<UserModel> GetCurrentUserAsync()
        {
            if (_currentUser != null) return _currentUser;

            if (string.IsNullOrEmpty(_idToken))
                throw new Exception("Niste prijavljeni.");

            // Trebalo bi učitati UID iz SecureStorage-a
            var uid = await SecureStorage.Default.GetAsync("user_uid");

            if (string.IsNullOrEmpty(uid))
                throw new Exception("Ne mogu dohvatiti korisnika.");

            // Dohvati user podatke iz DB
            var dbResponse = await _httpClient.GetAsync($"{FIREBASE_DB_URL}korisnici/{uid}.json?auth={_idToken}");
            dbResponse.EnsureSuccessStatusCode();
            var userJson = await dbResponse.Content.ReadAsStringAsync();

            _currentUser = JsonSerializer.Deserialize<UserModel>(userJson)!;
            _currentUser.Uid = uid;

            return _currentUser;
        }
        #endregion

        #region Update User
        public async Task UpdateUserAsync(UserModel user)
        {
            if (string.IsNullOrEmpty(_idToken))
                throw new Exception("Niste prijavljeni.");

            var dbContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var dbResponse = await _httpClient.PutAsync($"{FIREBASE_DB_URL}korisnici/{user.Uid}.json?auth={_idToken}", dbContent);
            dbResponse.EnsureSuccessStatusCode();

            _currentUser = user;
            await SaveToStorageAsync();
        }
        #endregion

        #region Logout
        public async Task LogoutAsync()
        {
            _idToken = null;
            _currentUser = null;
            await SecureStorage.Default.SetAsync("auth_token", "");
            await SecureStorage.Default.SetAsync("user_uid", ""); // ← Dodato
        }
        #endregion

        private class FirebaseAuthResponse
        {
            public string? idToken { get; set; }
            public string? localId { get; set; }
            public string? email { get; set; }
        }
    }
}