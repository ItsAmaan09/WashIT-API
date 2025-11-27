using System.ComponentModel.DataAnnotations;

namespace washit.dtos
{
    public class CancelReservationDto
    {
        [Required]
        public int ReservationId { get; set; }
    }
}