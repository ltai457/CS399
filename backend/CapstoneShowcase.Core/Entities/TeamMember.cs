namespace CapstoneShowcase.Core.Entities
{
    public class TeamMember
    {
        public int TeamId { get; set; }
        public string UserId { get; set; }
        public string Role { get; set; } // "Leader" or "Member"
        
        // Navigation properties
        public virtual Team Team { get; set; }
        public virtual User User { get; set; }
    }
}
