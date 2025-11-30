using System.ComponentModel.DataAnnotations;

namespace washit.dtos
{
    public class JoinWaitingListDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "WashTypeId must be greater than 0")]
        public int WashTypeId { get; set; }
    }
}