using Audiophiles_API.DTOs.Auth;

namespace Audiophiles_API.IServices
{
    public interface IAudioAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model); // Register
        Task<AuthModel> LoginAsync(LoginModel model); // Login
    }
}
