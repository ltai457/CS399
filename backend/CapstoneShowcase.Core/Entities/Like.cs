using System;

namespace CapstoneShowcase.Core.Entities
{
    public class Like
    {
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }
}