using washit.models;

namespace washit.repository
{
    public interface IReservationRepository
    {
        Task<Reservation?> GetActiveMachineAsync(int washTypeId);
        Task<int> CreateReservationAsync(Reservation reservation);
        Task<bool> CancelReservationAsync(int reservationId);
        Task<int> AddToWaitingListAsync(WaitingListEntry entry);
        Task MarkUserAsNotifiedAsync(int id);
        Task<WaitingListEntry?> GetNextWaitingUserAsync(int washTypeId);
        Task<Reservation?> GetReservationByIdAsync(int reservationId);
        Task<IEnumerable<Machine>> GetAllActiveMachineAsync();
        Task<Reservation?> GetActiveReservationByMachineIdAsync(int id);
        Task<Reservation?> GetReservationByMachineIdAsync(int machineId, string userName);

    }
}