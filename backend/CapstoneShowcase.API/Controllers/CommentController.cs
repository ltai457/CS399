using CapstoneShowcase.Core.DTOs.Comments;
using CapstoneShowcase.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CapstoneShowcase.API.Controllers
{
    [Route("api/projects/{projectId}/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        
        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetProjectComments(int projectId)
        {
            try
            {
                string userId = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : null;
                var comments = await _commentService.GetProjectCommentsAsync(projectId, userId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(int projectId, [FromBody] CreateCommentDto createCommentDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var comment = await _commentService.AddCommentAsync(projectId, createCommentDto, userId);
                return CreatedAtAction(nameof(GetProjectComments), new { projectId }, comment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize]
        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment(int projectId, int commentId, [FromBody] UpdateCommentDto updateCommentDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var comment = await _commentService.UpdateCommentAsync(commentId, updateCommentDto, userId);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize]
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(int projectId, int commentId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _commentService.DeleteCommentAsync(commentId, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}