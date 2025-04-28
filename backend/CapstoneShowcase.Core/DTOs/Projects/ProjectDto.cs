using System;
using System.Collections.Generic;

namespace CapstoneShowcase.Core.DTOs.Projects
{
    public class ProjectDto
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
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Related data
        public TeamDto Team { get; set; }
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        public List<BadgeDto> Badges { get; set; }
        public bool CurrentUserLiked { get; set; }
    }
    
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TeamMemberDto> Members { get; set; }
    }
    
    public class TeamMemberDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public string Role { get; set; }
    }
    
    public class BadgeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public DateTime AwardedAt { get; set; }
    }
}