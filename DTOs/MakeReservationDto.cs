using System.ComponentModel.DataAnnotations;

namespace washit.dtos
{
    public class MakeReservationDto
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(4)]
        public string UserName { get; set; }

        [Required]
        public int WashTypeId { get; set; }
    }
}