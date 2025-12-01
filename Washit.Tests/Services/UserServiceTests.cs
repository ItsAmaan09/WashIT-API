using Moq;
using washit.models;
using washit.repository;
using washit.services;

namespace washit.tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;
        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public async Task GetUserAsync_ShouldReturnUser_WhenCredentialsAreCorrect()
        {
            // Arrange
            var expectedUser = new User {Id = 1, UserName = "test"};
            _mockUserRepository.Setup(x => x.GetUserAsync("test","123")).ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserAsync("test", "123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test", result.UserName);
            _mockUserRepository.Verify(x => x.GetUserAsync("test", "123"), Times.Once);
        }

        [Fact]
        public async Task GetUserAsync_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            _mockUserRepository.Setup(x => x.GetUserAsync("sa","123")).ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetUserAsync("sa","123");

            // Assert
            Assert.Null(result);
        }
    }
}