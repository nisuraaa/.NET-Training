using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthenticationService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<Role> roleManager,
            IJwtTokenService jwtTokenService,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null || !user.IsActive)
                {
                    _logger.LogWarning("Login attempt failed: User not found or inactive - {Email}", request.Email);
                    return null;
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Login attempt failed: Invalid credentials - {Email}", request.Email);
                    return null;
                }

                // Update last login
                user.LastLoginAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                var roles = await _userManager.GetRolesAsync(user);
                return _jwtTokenService.CreateAuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, roles.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {Email}", request.Email);
                return null;
            }
        }

        public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration failed: User already exists - {Email}", request.Email);
                    return null;
                }

                var user = new User
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    EmailConfirmed = true // For demo purposes
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Registration failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    return null;
                }

                // Assign role
                var roleExists = await _roleManager.RoleExistsAsync(request.Role);
                if (roleExists)
                {
                    await _userManager.AddToRoleAsync(user, request.Role);
                }
                else
                {
                    // Create role if it doesn't exist
                    await _roleManager.CreateAsync(new Role { Name = request.Role });
                    await _userManager.AddToRoleAsync(user, request.Role);
                }

                var roles = await _userManager.GetRolesAsync(user);
                return _jwtTokenService.CreateAuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, roles.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user {Email}", request.Email);
                return null;
            }
        }

        public async Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                var principal = _jwtTokenService.GetPrincipalFromExpiredToken(request.Token);
                if (principal == null)
                {
                    _logger.LogWarning("Refresh token failed: Invalid token");
                    return null;
                }

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    _logger.LogWarning("Refresh token failed: User ID not found in token");
                    return null;
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null || !user.IsActive)
                {
                    _logger.LogWarning("Refresh token failed: User not found or inactive - {UserId}", userId);
                    return null;
                }

                // In a real application, you would validate the refresh token against a database
                // For this demo, we'll just generate a new token
                var roles = await _userManager.GetRolesAsync(user);
                return _jwtTokenService.CreateAuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, roles.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return null;
            }
        }

        public async Task<bool> LogoutAsync(string userId)
        {
            try
            {
                // In a real application, you would invalidate the refresh token in the database
                // For this demo, we'll just return true
                _logger.LogInformation("User logged out - {UserId}", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout for user {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var principal = _jwtTokenService.GetPrincipalFromExpiredToken(token);
                if (principal == null)
                {
                    return false;
                }

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return false;
                }

                var user = await _userManager.FindByIdAsync(userId);
                return user != null && user.IsActive;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                return false;
            }
        }
    }
}
