using System.ComponentModel.DataAnnotations;

namespace washit.dtos
{
    public class MakeReservationDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Machine Id must be greater than 0")]
        public int MachineId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "WashType Id must be greater than 0")]
        public int WashTypeId { get; set; }
    }
}