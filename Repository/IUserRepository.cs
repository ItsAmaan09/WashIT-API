using washit.models;

namespace washit.repository
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync(string username, string password);
        Task<int> RegisterUserAsync(string userName, string password);
        Task<User?> GetUserByNameAsync(string userName);
    }
}