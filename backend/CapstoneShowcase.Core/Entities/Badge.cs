namespace CapstoneShowcase.Core.Entities
{
    public class Badge
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        
        // Navigation properties
        public virtual ICollection<ProjectBadge> Projects { get; set; }
    }
}