using System;
using System.Collections.Generic;
using System.Text.Json;

namespace CapstoneShowcase.Core.Entities
{
    public class Student
    {
        public string UserId { get; set; }
        public string SkillsJson { get; set; }
        public string InterestsJson { get; set; }
        public string GithubProfile { get; set; }
        public string LinkedInProfile { get; set; }
        public string Biography { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<ProjectBid> ProjectBids { get; set; }
        public virtual TeamFormationProfile TeamFormationProfile { get; set; }
        
        // Helper methods for JSON properties
        public List<string> Skills 
        { 
            get => string.IsNullOrEmpty(SkillsJson) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(SkillsJson);
            set => SkillsJson = JsonSerializer.Serialize(value);
        }
        
        public List<string> Interests
        {
            get => string.IsNullOrEmpty(InterestsJson) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(InterestsJson);
            set => InterestsJson = JsonSerializer.Serialize(value);
        }
    }
}