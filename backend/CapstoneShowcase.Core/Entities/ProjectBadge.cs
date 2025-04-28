using System;

namespace CapstoneShowcase.Core.Entities
{
    public class ProjectBadge
    {
        public int ProjectId { get; set; }
        public int BadgeId { get; set; }
        public string AwardedByUserId { get; set; }
        public DateTime AwardedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Project Project { get; set; }
        public virtual Badge Badge { get; set; }
        public virtual User AwardedBy { get; set; }
    }
}
