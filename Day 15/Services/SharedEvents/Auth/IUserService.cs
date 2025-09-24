using SharedEvents.Auth;

namespace SharedEvents.Auth
{
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(string id);
        Task<bool> ValidatePasswordAsync(User user, string password);
        Task<User> CreateUserAsync(string email, string password, string firstName, string lastName, List<string> roles);
        Task UpdateLastLoginAsync(string userId);
    }
}
