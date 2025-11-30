using washit.models;

namespace washit.services
{
    public interface IReservationService
    {
        Task<Reservation?> ReserveMachineAsync(int? userId, int washTypeId);
        Task<bool> CancelReservationAsync(int reservationId,int? userId);
        Task<int> JoinWaitingListAsync(string userName, int washTypeId);
        Task<IEnumerable<Machine>> GetMachines();
        Task<string> CheckMachineAvailability(int id, string userName);
        Task<Reservation?> GetReservationByMachineIdAsync(int machineId, string userName);
        Task<Reservation?> GetUserActiveReservationAsync(int? userId);
    }
}
