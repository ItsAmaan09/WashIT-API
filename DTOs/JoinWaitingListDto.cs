using System.ComponentModel.DataAnnotations;

namespace washit.dtos
{
    public class JoinWaitingListDto
    {
        [Required]
        [MinLength(4)]
        public string UserName { get; set; }

        [Required]
        public int WashTypeId { get; set; }
    }
}