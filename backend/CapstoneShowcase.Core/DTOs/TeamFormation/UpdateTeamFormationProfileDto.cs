using System.Collections.Generic;

namespace CapstoneShowcase.Core.DTOs.TeamFormation
{
    public class UpdateTeamFormationProfileDto
    {
        public List<string> Skills { get; set; }
        public List<string> Interests { get; set; }
        public bool LookingForTeam { get; set; }
    }
}