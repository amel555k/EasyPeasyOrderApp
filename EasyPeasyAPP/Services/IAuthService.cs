using EasyPeasyAPP.Models;

namespace EasyPeasyAPP.Services
{
    public interface IAuthService
    {
        Task<UserModel> RegisterWithEmailAsync(string email, string password, string ime, string? telefon = null, string? adresa = null);
        Task<UserModel> LoginWithEmailAsync(string email, string password);
       
        Task<UserModel> GetCurrentUserAsync();
        Task UpdateUserAsync(UserModel user);

        Task LogoutAsync();
        Task InitializeAsync();
        bool IsAuthenticated { get; }
        UserModel? CurrentUser { get; }
    }
}
