using Microsoft.AspNetCore.Http;

namespace CapstoneShowcase.Core.DTOs.Projects
{
    public class UpdateProjectDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Repository { get; set; }
        public string DemoUrl { get; set; }
        public IFormFile Image { get; set; }
        public string Semester { get; set; }
        public int? Year { get; set; }
        public int? TeamId { get; set; }
    }
}
