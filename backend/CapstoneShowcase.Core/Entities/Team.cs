using System;
using System.Collections.Generic;

namespace CapstoneShowcase.Core.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<TeamMember> Members { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}