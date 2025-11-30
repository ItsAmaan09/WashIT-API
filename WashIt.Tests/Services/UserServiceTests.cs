using Xunit;
using Moq;
using FluentAssertions;
using washit.services;
using washit.repository;
using washit.models;
using washit.dtos;

namespace WashIt.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repo;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _repo = new Mock<IUserRepository>();
            _service = new UserService(_repo.Object);
        }

        [Fact]
        public async Task RegisterUser_ShouldThrow_WhenUserExists()
        {
            _repo.Setup(r => r.GetUserByNameAsync("virat")).ReturnsAsync(new User { UserName = "virat" });

            Func<Task> act = async () =>
            {
                await _service.RegisterUserAsync(new RegisterDto
                {
                    UserName = "virat",
                    Password = "123"
                });
            };

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Username already exists");
        }
    }
}
