using washit.models;

namespace washit.services
{
    public interface IReservationService
    {
        Task<Reservation?> ReserveMachineAsync(int? userId, int washTypeId);
        Task<bool> CancelReservationAsync(int reservationId,int? userId);
        Task<int> JoinWaitingListAsync(int? userId, int washTypeId);
        Task<IEnumerable<Machine>> GetMachines();
        Task<Reservation?> GetUserActiveReservationAsync(int? userId);
        Task<List<Machine>> GetMachinesWithStatusAsync(int userId);
    }
}
