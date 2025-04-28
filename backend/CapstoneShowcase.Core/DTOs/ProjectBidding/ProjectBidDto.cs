namespace CapstoneShowcase.Core.DTOs.ProjectBidding
{
    public class ProjectBidDto
    {
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectShortDescription { get; set; }
        public int Preference { get; set; }
        public string Reason { get; set; }
    }
}