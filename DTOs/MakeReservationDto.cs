using System.ComponentModel.DataAnnotations;

namespace washit.dtos
{
    public class MakeReservationDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int WashTypeId { get; set; }
    }
}