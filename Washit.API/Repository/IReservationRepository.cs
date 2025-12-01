using washit.models;

namespace washit.repository
{
    public interface IReservationRepository
    {
        Task<Machine?> GetActiveMachineAsync(int washTypeId, int machineId);
        Task<int> CreateReservationAsync(Reservation reservation);
        Task<bool> CancelReservationAsync(int reservationId, int? userId);
        Task<int> AddToWaitingListAsync(WaitingListEntry entry);
        Task MarkUserAsNotifiedAsync(int id);
        Task<WaitingListEntry?> GetNextWaitingUserAsync(int washTypeId, int machineId);
        Task<Reservation?> GetReservationByIdAsync(int reservationId);
        Task<IEnumerable<Machine>> GetAllActiveMachineAsync();
        Task<Reservation?> GetActiveReservationByMachineIdAsync(int id);
        Task<Reservation?> GetUserActiveReservationAsync(int? userId);
        Task<bool> CheckActiveWaitlistExist(int machineId, int? userId);

    }
}