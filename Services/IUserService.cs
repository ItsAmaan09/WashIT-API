using washit.dtos;
using washit.models;

namespace washit.services
{
    public interface IUserService
    {
        Task<User?> GetUserAsync(string userName, string password);
        Task<int> RegisterUserAsync(RegisterDto registerDto);
        Task<User?> GetUserByNameAsync(string userName);
    }
}