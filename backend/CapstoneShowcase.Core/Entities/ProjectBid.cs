namespace CapstoneShowcase.Core.Entities
{
    public class ProjectBid
    {
        public string StudentId { get; set; }
        public int ProjectId { get; set; }
        public int Preference { get; set; } // 1 for first choice, 2 for second, etc.
        public string Reason { get; set; }
        
        // Navigation properties
        public virtual Student Student { get; set; }
        public virtual Project Project { get; set; }
    }
}