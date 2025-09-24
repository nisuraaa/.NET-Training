using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SharedEvents.Auth
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfiguration _jwtConfig;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public JwtService(JwtConfiguration jwtConfig)
        {
            _jwtConfig = jwtConfig;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new("jti", Guid.NewGuid().ToString())
            };

            // Add role claims
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfig.ExpirationMinutes),
                signingCredentials: credentials
            );

            return _tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey)),
                ValidateLifetime = false, // We don't care about the token expiry
                ValidIssuer = _jwtConfig.Issuer,
                ValidAudience = _jwtConfig.Audience
            };

            try
            {
                var principal = _tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                
                if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public bool ValidateToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey)),
                ValidateLifetime = true,
                ValidIssuer = _jwtConfig.Issuer,
                ValidAudience = _jwtConfig.Audience,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                _tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
