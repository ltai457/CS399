using System;
using System.Collections.Generic;

namespace CapstoneShowcase.Core.DTOs.Teams
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
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
}
