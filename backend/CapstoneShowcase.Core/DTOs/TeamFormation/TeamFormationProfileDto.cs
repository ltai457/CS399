using System;
using System.Collections.Generic;

namespace CapstoneShowcase.Core.DTOs.TeamFormation
{
    public class TeamFormationProfileDto
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Interests { get; set; }
        public string GithubProfile { get; set; }
        public string LinkedInProfile { get; set; }
        public string Biography { get; set; }
        public bool LookingForTeam { get; set; }
    }
}