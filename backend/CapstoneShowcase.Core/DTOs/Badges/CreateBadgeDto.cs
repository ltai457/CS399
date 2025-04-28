using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CapstoneShowcase.Core.DTOs.Badges
{
    public class CreateBadgeDto
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public IFormFile Image { get; set; }
    }
}