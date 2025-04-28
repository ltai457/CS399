using System.ComponentModel.DataAnnotations;

namespace CapstoneShowcase.Core.DTOs.ProjectBidding
{
    public class CreateProjectBidDto
    {
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        [Range(1, 10)]
        public int Preference { get; set; }
        
        public string Reason { get; set; }
    }
}