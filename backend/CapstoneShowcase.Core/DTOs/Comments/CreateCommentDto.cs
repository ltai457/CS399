using System.ComponentModel.DataAnnotations;

namespace CapstoneShowcase.Core.DTOs.Comments
{
    public class CreateCommentDto
    {
        [Required]
        public string Content { get; set; }
    }
}