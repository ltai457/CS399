using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CapstoneShowcase.Core.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePicture { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Student Student { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<TeamMember> TeamMemberships { get; set; }
        public virtual ICollection<ProjectBadge> AwardedBadges { get; set; }
    }
}