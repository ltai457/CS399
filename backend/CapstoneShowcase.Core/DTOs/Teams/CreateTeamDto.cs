using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CapstoneShowcase.Core.DTOs.Teams
{
    public class CreateTeamDto
    {
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        // User IDs for team members (current user will be added as leader automatically)
        public List<string> MemberIds { get; set; }
    }
}