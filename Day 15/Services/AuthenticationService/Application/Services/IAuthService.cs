using AuthenticationService.Domain.Models;

namespace AuthenticationService.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(LoginRequest request);
        Task<AuthResponse?> RegisterAsync(RegisterRequest request);
        Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request);
        Task<bool> LogoutAsync(string userId);
        Task<bool> ValidateTokenAsync(string token);
    }
}
