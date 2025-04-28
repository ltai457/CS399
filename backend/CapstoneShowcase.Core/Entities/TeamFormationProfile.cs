using System;
using System.Text.Json;
using System.Collections.Generic;

namespace CapstoneShowcase.Core.Entities
{
    public class TeamFormationProfile
    {
        public string StudentId { get; set; }
        public string InterestsJson { get; set; }
        public string SkillsJson { get; set; }
        public bool LookingForTeam { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Student Student { get; set; }
        
        // Helper methods for JSON properties
        public List<string> Interests
        {
            get => string.IsNullOrEmpty(InterestsJson) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(InterestsJson);
            set => InterestsJson = JsonSerializer.Serialize(value);
        }
        
        public List<string> Skills
        {
            get => string.IsNullOrEmpty(SkillsJson) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(SkillsJson);
            set => SkillsJson = JsonSerializer.Serialize(value);
        }
    }
}