using System.Security.Cryptography;
using System.Text;

namespace SharedEvents.Auth
{
    public class UserService : IUserService
    {
        private readonly Dictionary<string, User> _users = new();
        private readonly IJwtService _jwtService;

        public UserService(IJwtService jwtService)
        {
            _jwtService = jwtService;
            InitializeDefaultUsers();
        }

        private void InitializeDefaultUsers()
        {
            // Create default users for testing
            var adminUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = "admin@company.com",
                PasswordHash = HashPassword("Admin123!"),
                Roles = new List<string> { UserRoles.Admin },
                FirstName = "Admin",
                LastName = "User",
                CreatedAt = DateTime.UtcNow
            };

            var managerUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = "manager@company.com",
                PasswordHash = HashPassword("Manager123!"),
                Roles = new List<string> { UserRoles.Manager },
                FirstName = "Manager",
                LastName = "User",
                CreatedAt = DateTime.UtcNow
            };

            var employeeUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = "employee@company.com",
                PasswordHash = HashPassword("Employee123!"),
                Roles = new List<string> { UserRoles.Employee },
                FirstName = "Employee",
                LastName = "User",
                CreatedAt = DateTime.UtcNow
            };

            var readOnlyUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = "readonly@company.com",
                PasswordHash = HashPassword("ReadOnly123!"),
                Roles = new List<string> { UserRoles.ReadOnly },
                FirstName = "ReadOnly",
                LastName = "User",
                CreatedAt = DateTime.UtcNow
            };

            _users[adminUser.Email] = adminUser;
            _users[managerUser.Email] = managerUser;
            _users[employeeUser.Email] = employeeUser;
            _users[readOnlyUser.Email] = readOnlyUser;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            await Task.Delay(1); // Simulate async operation
            return _users.TryGetValue(email, out var user) ? user : null;
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            await Task.Delay(1); // Simulate async operation
            return _users.Values.FirstOrDefault(u => u.Id == id);
        }

        public async Task<bool> ValidatePasswordAsync(User user, string password)
        {
            await Task.Delay(1); // Simulate async operation
            return user.PasswordHash == HashPassword(password);
        }

        public async Task<User> CreateUserAsync(string email, string password, string firstName, string lastName, List<string> roles)
        {
            await Task.Delay(1); // Simulate async operation
            
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                PasswordHash = HashPassword(password),
                Roles = roles,
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = DateTime.UtcNow
            };

            _users[email] = user;
            return user;
        }

        public async Task UpdateLastLoginAsync(string userId)
        {
            await Task.Delay(1); // Simulate async operation
            var user = _users.Values.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.LastLoginAt = DateTime.UtcNow;
            }
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
