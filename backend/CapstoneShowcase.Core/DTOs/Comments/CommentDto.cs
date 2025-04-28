using System;

namespace CapstoneShowcase.Core.DTOs.Comments
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // User information
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserProfilePicture { get; set; }
        
        // Whether the current user can edit/delete this comment
        public bool CanModify { get; set; }
    }
}