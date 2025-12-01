using washit.models;

namespace washit.services
{
    public interface IReservationService
    {
        Task<Reservation?> ReserveMachineAsync(int? userId, int washTypeId, int machineId);
        Task<bool> CancelReservationAsync(int reservationId,int? userId, int machineId);
        Task<int> JoinWaitingListAsync(int? userId, int washTypeId, int machineId);
        Task<IEnumerable<Machine>> GetMachines();
        Task<Reservation?> GetUserActiveReservationAsync(int? userId);
        Task<List<Machine>> GetMachinesWithStatusAsync(int userId);
        Task<bool> CheckActiveWaitlistExist(int machineId, int? userId);
    }
}
