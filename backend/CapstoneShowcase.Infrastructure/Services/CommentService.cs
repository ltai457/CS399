// CapstoneShowcase.Infrastructure/Services/CommentService.cs
using CapstoneShowcase.Core.DTOs.Comments;
using CapstoneShowcase.Core.Entities;
using CapstoneShowcase.Core.Interfaces;
using CapstoneShowcase.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneShowcase.Infrastructure.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public CommentService(
            ICommentRepository commentRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            UserManager<User> userManager)
        {
            _commentRepository = commentRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<CommentDto>> GetProjectCommentsAsync(int projectId, string currentUserId = null)
        {
            var comments = await _commentRepository.GetCommentsByProjectIdAsync(projectId);

            var isAdmin = !string.IsNullOrEmpty(currentUserId) && 
                await _userRepository.IsUserInRoleAsync(currentUserId, "Admin");
                
            return comments.Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                UserId = c.UserId,
                UserFullName = $"{c.User.FirstName} {c.User.LastName}",
                UserProfilePicture = c.User.ProfilePicture,
                CanModify = !string.IsNullOrEmpty(currentUserId) && 
                           (c.UserId == currentUserId || isAdmin)
            });
        }

        public async Task<CommentDto> AddCommentAsync(int projectId, CreateCommentDto createCommentDto, string userId)
        {
            // Check if project exists
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new Exception($"Project with ID {projectId} not found");

            // Get user
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            // Create comment
            var comment = new Comment
            {
                ProjectId = projectId,
                UserId = userId,
                Content = createCommentDto.Content,
                CreatedAt = DateTime.UtcNow
            };

            await _commentRepository.AddAsync(comment);

            // Return DTO
            return new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = null,
                UserId = userId,
                UserFullName = $"{user.FirstName} {user.LastName}",
                UserProfilePicture = user.ProfilePicture,
                CanModify = true
            };
        }

        public async Task<CommentDto> UpdateCommentAsync(int commentId, UpdateCommentDto updateCommentDto, string userId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
                throw new Exception($"Comment with ID {commentId} not found");

            // Check if user is allowed to update the comment
            var isOwner = comment.UserId == userId;
            var isAdmin = await _userRepository.IsUserInRoleAsync(userId, "Admin");

            if (!isOwner && !isAdmin)
                throw new Exception("You are not authorized to update this comment");

            // Update content
            comment.Content = updateCommentDto.Content;
            comment.UpdatedAt = DateTime.UtcNow;

            await _commentRepository.UpdateAsync(comment);

            // Get user info for response
            var user = await _userManager.FindByIdAsync(comment.UserId);

            // Return DTO
            return new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                UserId = comment.UserId,
                UserFullName = $"{user.FirstName} {user.LastName}",
                UserProfilePicture = user.ProfilePicture,
                CanModify = isOwner || isAdmin
            };
        }

        public async Task DeleteCommentAsync(int commentId, string userId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
                throw new Exception($"Comment with ID {commentId} not found");

            // Check if user is allowed to delete the comment
            var isOwner = comment.UserId == userId;
            var isAdmin = await _userRepository.IsUserInRoleAsync(userId, "Admin");

            if (!isOwner && !isAdmin)
                throw new Exception("You are not authorized to delete this comment");

            await _commentRepository.DeleteAsync(comment);
        }
    }
}