using System;

namespace CapstoneShowcase.Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }
}
