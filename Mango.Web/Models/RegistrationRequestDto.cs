using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class RegistrationRequestDto
    {
        [Required]
        public string Email { get; set; } = default!;
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string PhoneNumber { get; set; } = default!;
        [Required]
        public string Password { get; set; } = default!;
        public string? Role { get; set; }
    }
}
