using Xunit;
using Moq;
using FluentAssertions;
using washit.services;
using washit.repository;
using washit.models;
using washit.dtos;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace WashIt.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repo;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _repo = new Mock<IUserRepository>();

            // Mock HttpContextAccessor
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserId", "1"),
                new Claim(ClaimTypes.Name, "virat")
            }, "mock"));

            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(new DefaultHttpContext
            {
                User = user
            });

            // Inject mocks into service
            _service = new UserService(_repo.Object, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task RegisterUser_ShouldThrow_WhenUserExists()
        {
            // Arrange
            _repo.Setup(r => r.GetUserByNameAsync("virat"))
                .ReturnsAsync(new User { UserName = "virat" });

            var registerDto = new RegisterDto
            {
                UserName = "virat",
                Password = "123"
            };

            // Act
            Func<Task> act = async () => await _service.RegisterUserAsync(registerDto);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Username already exists");
        }

        [Fact]
        public async Task RegisterUser_ShouldSucceed_WhenUserDoesNotExist()
        {
            // Arrange
            _repo.Setup(r => r.GetUserByNameAsync("newuser")).ReturnsAsync((User?)null);
            _repo.Setup(r => r.RegisterUserAsync("newuser", "password")).ReturnsAsync(1);

            var registerDto = new RegisterDto
            {
                UserName = "newuser",
                Password = "password"
            };

            // Act
            var result = await _service.RegisterUserAsync(registerDto);

            // Assert
            result.Should().Be(1);
        }
    }
}
