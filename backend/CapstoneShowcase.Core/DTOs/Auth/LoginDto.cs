using System.ComponentModel.DataAnnotations;

namespace CapstoneShowcase.Core.DTOs.Auth
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}