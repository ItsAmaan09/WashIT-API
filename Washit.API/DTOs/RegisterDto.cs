using System.ComponentModel.DataAnnotations;

namespace washit.dtos
{
    public class RegisterDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "At least 3 character is required")]
        public string UserName { get; set; }
        
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 character")]
        public string Password { get; set; }
    }
}