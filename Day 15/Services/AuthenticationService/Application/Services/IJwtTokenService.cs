using AuthenticationService.Domain.Models;
using System.Security.Claims;

namespace AuthenticationService.Application.Services
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(string userId, string email, List<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        AuthResponse CreateAuthResponse(string userId, string email, string firstName, string lastName, List<string> roles);
    }
}
