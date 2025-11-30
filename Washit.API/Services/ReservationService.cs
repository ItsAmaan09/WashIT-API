using System.Security.Claims;
using washit.models;
using washit.repository;

namespace washit.services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReservationService(IReservationRepository repo, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Reservation?> ReserveMachineAsync(int? userId, int washTypeId)
        {
            try
            {
                if (userId == null || userId == 0)
                {
                    throw new Exception("Please Provide User Id");
                }

                var machine = await _repo.GetActiveMachineAsync(washTypeId);

                if (machine == null)
                {
                    throw new Exception("No available machine found for this wash type.");
                }

                var existing = await _repo.GetUserActiveReservationAsync(userId);

                if (existing != null)
                {
                    throw new Exception("You already have an active reservation.");
                }

                var reservation = new Reservation
                {
                    MachineId = machine.Id,
                    UserId = userId.Value,
                    WashTypeId = washTypeId,
                    ReservedAt = DateTime.UtcNow,
                    CreatedBy = userId.ToString(),
                    IsActive = true
                };

                reservation.Id = await _repo.CreateReservationAsync(reservation);

                return reservation;
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        public async Task<bool> CancelReservationAsync(int reservationId, int? userId)
        {
            try
            {
                if (userId == null || userId == 0)
                {
                    throw new Exception("User Id is required");
                }
                var reservation = await _repo.GetReservationByIdAsync(reservationId);

                if (reservation == null || !reservation.IsActive)
                {
                    throw new Exception("No Active Reservation found.");
                }

                if (reservation.UserId != userId)
                {
                    throw new Exception("You cannot cancel someone else's reservation.");
                }

                // Cancel reservation
                bool cancelled = await _repo.CancelReservationAsync(reservationId, userId);

                if (!cancelled)
                {
                    throw new Exception("Failed to cancel the reservation.");
                }

                // Notify next user in waiting list
                var nextUser = await _repo.GetNextWaitingUserAsync(reservation.WashTypeId);

                if (nextUser != null)
                {
                    await _repo.MarkUserAsNotifiedAsync(nextUser.Id);
                }
                return true;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<int> JoinWaitingListAsync(int? userId, int washTypeId)
        {
            try
            {
                if (userId == null || userId == 0)
                {
                    throw new Exception("User Id is required");
                }

                return await _repo.AddToWaitingListAsync(new WaitingListEntry
                {
                    UserId = userId.Value,
                    WashTypeId = washTypeId,
                    CreatedBy = userId.ToString()
                });

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<Machine>> GetMachines()
        {
            try
            {
                return await _repo.GetAllActiveMachineAsync();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<Reservation?> GetUserActiveReservationAsync(int? userId)
        {
            return await _repo.GetUserActiveReservationAsync(userId);
        }

        public async Task<List<Machine>> GetMachinesWithStatusAsync(int userId)
        {
            var machines = await this.GetMachines();

            var machineStatusList = new List<Machine>();

            foreach (var m in machines)
            {
                var reservation = await _repo.GetActiveReservationByMachineIdAsync(m.Id);

                string status;
                int? reservationId = null;

                if (reservation == null)
                {
                    status = "Available";
                }
                else if (reservation.UserId == userId)
                {
                    status = "Reserved by you";
                    reservationId = reservation.Id;
                }
                else
                {
                    status = "Reserved by other";
                    reservationId = reservation.Id;
                }

                machineStatusList.Add(new Machine
                {
                    Id = m.Id,
                    MachineName = m.MachineName,
                    WashTypeId = m.WashTypeId,
                    IsActive = m.IsActive,
                    Status = status,
                    ReservationId = reservationId
                });
            }
            return machineStatusList;
        }

    }
}
