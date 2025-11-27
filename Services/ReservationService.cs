using washit.models;
using washit.repository;

namespace washit.services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repo;

        public ReservationService(IReservationRepository repo)
        {
            _repo = repo;
        }

        public async Task<Reservation?> ReserveMachineAsync(string userName, int washTypeId)
        {
            try
            {
                var machine = await _repo.GetActiveMachineAsync(washTypeId);

                if (machine == null)
                    return null;

                var reservation = new Reservation
                {
                    MachineId = machine.Id,
                    UserName = userName,
                    WashTypeId = washTypeId,
                    ReservedAt = DateTime.UtcNow,
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

        public async Task<bool> CancelReservationAsync(int reservationId)
        {
            try
            {
                var reservation = await _repo.GetReservationByIdAsync(reservationId);
                if (reservation == null)
                    return false;

                // Cancel reservation
                bool cancelled = await _repo.CancelReservationAsync(reservationId);

                if (!cancelled)
                    return false;

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

        public async Task<int> JoinWaitingListAsync(string userName, int washTypeId)
        {
            try
            {
                return await _repo.AddToWaitingListAsync(new WaitingListEntry
                {
                    UserName = userName,
                    WashTypeId = washTypeId
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

        public async Task<string> CheckMachineAvailability(int id, string userName)
        {
            try
            {
                var reservation = await _repo.GetActiveReservationByMachineIdAsync(id);

                // No reservation found
                if (reservation == null)
                    return "Available";

                // Reserved by same user
                if (reservation.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase))
                    return "Reserved by you";

                // Reserved by someone else
                return "Reserve by other";
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<Reservation?> GetReservationByMachineIdAsync(int machineId, string userName)
        {
            return await _repo.GetReservationByMachineIdAsync(machineId, userName);
        }

    }
}
