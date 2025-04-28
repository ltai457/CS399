using System.ComponentModel.DataAnnotations;

namespace CapstoneShowcase.Core.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
        
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        // Optional fields for students
        public string[] Skills { get; set; }
        public string[] Interests { get; set; }
        public string GithubProfile { get; set; }
        public string LinkedInProfile { get; set; }
        public string Biography { get; set; }
        
        // Role will be determined by the registration endpoint used
        // (e.g., /auth/register/student, /auth/register/visitor)
    }
}