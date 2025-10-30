using EasyPeasyAPP.Models;

namespace EasyPeasyAPP.Services
{
    public interface IAuthService
    {
        Task<User> LoginWithEmailAsync(string email, string password);
        Task<User> RegisterWithEmailAsync(string email, string password, string displayName);
        Task LogoutAsync();
        Task<User?> GetCurrentUserAsync();
        bool IsAuthenticated { get; }
    }
}