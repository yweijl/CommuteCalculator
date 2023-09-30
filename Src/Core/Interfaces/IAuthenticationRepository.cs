using Core.Models;

namespace Core.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> SaveUserAsync(User user);
        Task<bool> UserExistsAsync(string email);
    }
}