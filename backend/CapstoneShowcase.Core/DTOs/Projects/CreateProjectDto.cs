using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CapstoneShowcase.Core.DTOs.Projects
{
    public class CreateProjectDto
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public string ShortDescription { get; set; }
        
        public string Repository { get; set; }
        
        public string DemoUrl { get; set; }
        
        public IFormFile Image { get; set; }
        
        [Required]
        public string Semester { get; set; }
        
        [Required]
        public int Year { get; set; }
        
        public int? TeamId { get; set; }
    }
}
