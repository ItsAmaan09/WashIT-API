using System.ComponentModel.DataAnnotations;

namespace washit.dtos
{
    public class CancelReservationDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Reservation Id must be greater than 0")]
        public int ReservationId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Machine Id must be greater than 0")]
        public int MachineId { get; set; }
    }
}