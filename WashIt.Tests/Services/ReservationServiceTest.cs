using Xunit;
using Moq;
using FluentAssertions;
using washit.services;
using washit.repository;
using washit.models;
using System.Threading.Tasks;
using System;

namespace WashIt.Tests.Services
{
    public class ReservationServiceTests
    {
        private readonly Mock<IReservationRepository> _repo;
        private readonly ReservationService _service;

        public ReservationServiceTests()
        {
            _repo = new Mock<IReservationRepository>();
            _service = new ReservationService(_repo.Object);
        }

        [Fact]
        public async Task ReserveMachineAsync_ShouldThrow_WhenUserHasActiveReservation()
        {
            int userId = 1;
            int washTypeId = 2;

            _repo.Setup(r => r.GetUserActiveReservationAsync(userId))
                .ReturnsAsync(new Reservation { Id = 5, UserId = userId, IsActive = true });

            Func<Task> act = async () => await _service.ReserveMachineAsync(userId, washTypeId);

            await act.Should().ThrowAsync<Exception>().WithMessage("You already have an active reservation.");
        }

        [Fact]
        public async Task ReserveMachineAsync_ShouldCreateReservation_WhenMachineIsFree()
        {
            int userId = 1;
            int washTypeId = 2;

            _repo.Setup(r => r.GetUserActiveReservationAsync(userId)).ReturnsAsync((Reservation)null);

            _repo.Setup(r => r.GetActiveMachineAsync(washTypeId)).ReturnsAsync(new Machine { Id = 3, WashTypeId = washTypeId });

            _repo.Setup(r => r.CreateReservationAsync(It.IsAny<Reservation>())).ReturnsAsync(100);

            var result = await _service.ReserveMachineAsync(userId, washTypeId);

            result.Id.Should().Be(100);
            result.MachineId.Should().Be(3);
            result.UserId.Should().Be(userId);
        }

    }
}
