using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SharedEvents.Auth
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("Jwt").Get<JwtConfiguration>() ?? new JwtConfiguration
            {
                SecretKey = "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
                Issuer = "MicroservicesAuth",
                Audience = "MicroservicesUsers",
                ExpirationMinutes = 60
            };

            services.AddSingleton(jwtConfig);
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserService, UserService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Admin policy - can do everything
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole(UserRoles.Admin));

                // Manager policy - can manage employees and projects
                options.AddPolicy("ManagerOrAdmin", policy =>
                    policy.RequireRole(UserRoles.Manager, UserRoles.Admin));

                // Employee policy - can view and edit their own data
                options.AddPolicy("EmployeeOrAbove", policy =>
                    policy.RequireRole(UserRoles.Employee, UserRoles.Manager, UserRoles.Admin));

                // Read-only policy - can only view data
                options.AddPolicy("ReadOnlyOrAbove", policy =>
                    policy.RequireRole(UserRoles.ReadOnly, UserRoles.Employee, UserRoles.Manager, UserRoles.Admin));

                // Write operations policy
                options.AddPolicy("WriteAccess", policy =>
                    policy.RequireRole(UserRoles.Employee, UserRoles.Manager, UserRoles.Admin));
            });

            return services;
        }
    }
}
