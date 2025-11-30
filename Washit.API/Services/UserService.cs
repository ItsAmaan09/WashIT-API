using washit.dtos;
using washit.models;
using washit.repository;

namespace washit.services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<User?> GetUserAsync(string userName, string password)
        {
            try
            {
                var user = _userRepository.GetUserAsync(userName, password);

                return user;

            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public Task<User?> GetUserByNameAsync(string userName)
        {
            try
            {
                return _userRepository.GetUserByNameAsync(userName);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<int> RegisterUserAsync(RegisterDto registerDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(registerDto.UserName) || string.IsNullOrWhiteSpace(registerDto.Password))
                {
                    throw new Exception("Username and password are required.");
                }

                var existingUser = await GetUserByNameAsync(registerDto.UserName);

                if (existingUser != null)
                {
                    throw new Exception("Username already exists");
                }

                int newUserId = await _userRepository.RegisterUserAsync(registerDto.UserName, registerDto.Password);

                return newUserId;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}