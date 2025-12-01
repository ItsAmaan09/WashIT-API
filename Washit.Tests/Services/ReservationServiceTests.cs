using Microsoft.AspNetCore.Http;
using Moq;
using washit.models;
using washit.repository;
using washit.services;

namespace washit.tests
{
    public class ReservationServiceTests
    {
        private readonly Mock<IReservationRepository> _mockReservationRepository;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly ReservationService _reservationService;
        public ReservationServiceTests()
        {
            _mockReservationRepository = new Mock<IReservationRepository>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _reservationService = new ReservationService(_mockReservationRepository.Object, _mockHttpContextAccessor.Object);
        }

        // -------------------------------------------------
        // TEST 1 : When UserId is null -> throws exception
        // -------------------------------------------------
        [Fact]
        public async Task ReserveMachineAsync_ShouldThrow_WhenUserIdIsNull()
        {
            var exception = await Assert.ThrowsAsync<Exception>(() => _reservationService.ReserveMachineAsync(null, 1));

            Assert.Equal("Please Provide User Id", exception.Message);
        }

        // -----------------------------------------------
        // TEST 2: When no machine available for wash type
        // -----------------------------------------------

        [Fact]
        public async Task ReserveMachineAsync_ShouldThrow_WhenNoMachineAvailable()
        {
            _mockReservationRepository.Setup(x => x.GetActiveMachineAsync(1)).ReturnsAsync((Machine?)null);

            var ex = await Assert.ThrowsAsync<Exception>(() => _reservationService.ReserveMachineAsync(1, 1));

            Assert.Equal("No available machine found for this wash type.", ex.Message);
        }

        // ----------------------------------------------
        // TEST 3: User already has an active reservation
        // ----------------------------------------------

        [Fact]
        public async Task ReserveMachineAsync_ShouldThrow_WhenUserAlreadyHasReservation()
        {
            _mockReservationRepository.Setup(x => x.GetActiveMachineAsync(1)).ReturnsAsync(new Machine { Id = 10 });

            _mockReservationRepository.Setup(x => x.GetUserActiveReservationAsync(1)).ReturnsAsync(new Reservation { Id = 99 });

            var ex = await Assert.ThrowsAsync<Exception>(() => _reservationService.ReserveMachineAsync(1,1));

            Assert.Equal("You already have an active reservation.", ex.Message);

        }
    }
}