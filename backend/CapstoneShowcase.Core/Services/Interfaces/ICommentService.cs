using CapstoneShowcase.Core.DTOs.Comments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapstoneShowcase.Core.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetProjectCommentsAsync(int projectId, string currentUserId = null);
        Task<CommentDto> AddCommentAsync(int projectId, CreateCommentDto createCommentDto, string userId);
        Task<CommentDto> UpdateCommentAsync(int commentId, UpdateCommentDto updateCommentDto, string userId);
        Task DeleteCommentAsync(int commentId, string userId);
    }
}