using System;
using System.Collections.Generic;

namespace CapstoneShowcase.Core.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Repository { get; set; }
        public string DemoUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Semester { get; set; }
        public int Year { get; set; }
        public int? TeamId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Team Team { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<ProjectBadge> Badges { get; set; }
        public virtual ICollection<ProjectBid> Bids { get; set; }
    }
}