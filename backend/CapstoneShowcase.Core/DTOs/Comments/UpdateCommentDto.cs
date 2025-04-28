using System.ComponentModel.DataAnnotations;

namespace CapstoneShowcase.Core.DTOs.Comments
{
    public class UpdateCommentDto
    {
        [Required]
        public string Content { get; set; }
    }
}