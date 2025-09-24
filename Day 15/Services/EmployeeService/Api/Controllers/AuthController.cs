using Microsoft.AspNetCore.Mvc;
using SharedEvents.Auth;

namespace EmployeeServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, IJwtService jwtService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userService.GetUserByEmailAsync(request.Email);
                if (user == null)
                {
                    _logger.LogWarning("Login attempt with non-existent email: {Email}", request.Email);
                    return Unauthorized("Invalid email or password");
                }

                var isValidPassword = await _userService.ValidatePasswordAsync(user, request.Password);
                if (!isValidPassword)
                {
                    _logger.LogWarning("Invalid password attempt for user: {Email}", request.Email);
                    return Unauthorized("Invalid email or password");
                }

                var token = _jwtService.GenerateToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken();

                await _userService.UpdateLastLoginAsync(user.Id);

                var response = new LoginResponse
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    Expiration = DateTime.UtcNow.AddMinutes(60), // Should match JWT config
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = user.Roles
                };

                _logger.LogInformation("User {Email} logged in successfully", user.Email);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user: {Email}", request.Email);
                return StatusCode(500, "An error occurred during login");
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var principal = _jwtService.GetPrincipalFromExpiredToken(request.Token);
                if (principal == null)
                {
                    return Unauthorized("Invalid token");
                }

                var userId = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Invalid token");
                }

                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return Unauthorized("User not found");
                }

                var newToken = _jwtService.GenerateToken(user);
                var newRefreshToken = _jwtService.GenerateRefreshToken();

                var response = new LoginResponse
                {
                    Token = newToken,
                    RefreshToken = newRefreshToken,
                    Expiration = DateTime.UtcNow.AddMinutes(60),
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = user.Roles
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return StatusCode(500, "An error occurred during token refresh");
            }
        }

        [HttpPost("validate")]
        public ActionResult ValidateToken([FromBody] ValidateTokenRequest request)
        {
            try
            {
                var isValid = _jwtService.ValidateToken(request.Token);
                return Ok(new { IsValid = isValid });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token validation");
                return StatusCode(500, "An error occurred during token validation");
            }
        }

        [HttpGet("test-users")]
        public ActionResult GetTestUsers()
        {
            var testUsers = new[]
            {
                new { Email = "admin@company.com", Password = "Admin123!", Role = "Admin" },
                new { Email = "manager@company.com", Password = "Manager123!", Role = "Manager" },
                new { Email = "employee@company.com", Password = "Employee123!", Role = "Employee" },
                new { Email = "readonly@company.com", Password = "ReadOnly123!", Role = "ReadOnly" }
            };

            return Ok(testUsers);
        }
    }

    public class RefreshTokenRequest
    {
        public string Token { get; set; } = string.Empty;
    }

    public class ValidateTokenRequest
    {
        public string Token { get; set; } = string.Empty;
    }
}
