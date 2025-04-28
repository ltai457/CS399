using CapstoneShowcase.Core.DTOs.Projects;
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
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStorageService _storageService;
        private readonly UserManager<User> _userManager;
        
        public ProjectService(
            IProjectRepository projectRepository,
            ITeamRepository teamRepository,
            IUserRepository userRepository,
            IStorageService storageService,
            UserManager<User> userManager)
        {
            _projectRepository = projectRepository;
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _storageService = storageService;
            _userManager = userManager;
        }
        
        public async Task<IEnumerable<ProjectDto>> GetProjectsAsync(int page = 1, int pageSize = 10)
        {
            var skip = (page - 1) * pageSize;
            var projects = await _projectRepository.GetProjectsWithDetailsAsync(skip, pageSize);
            
            return projects.Select(MapProjectToDto);
        }
        
        public async Task<ProjectDto> GetProjectByIdAsync(int id, string currentUserId = null)
        {
            var project = await _projectRepository.GetProjectByIdWithDetailsAsync(id);
            
            if (project == null)
                throw new Exception($"Project with ID {id} not found");
                
            var dto = MapProjectToDto(project);
            
            if (!string.IsNullOrEmpty(currentUserId))
            {
                dto.CurrentUserLiked = await _projectRepository.HasUserLikedProjectAsync(id, currentUserId);
            }
            
            return dto;
        }
        
        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto, string userId)
        {
            // Check if user is allowed to create a project
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user == null)
                throw new Exception("User not found");
                
            // If team ID is provided, check if user is a member of the team
            if (createProjectDto.TeamId.HasValue)
            {
                var isMember = await _teamRepository.IsUserTeamMemberAsync(createProjectDto.TeamId.Value, userId);
                
                if (!isMember)
                    throw new Exception("You are not a member of this team");
            }
            
            // Upload image if provided
            string imageUrl = null;
            if (createProjectDto.Image != null)
            {
                imageUrl = await _storageService.UploadFileAsync(createProjectDto.Image, "projects");
            }
            
            // Create project
            var project = new Project
            {
                Title = createProjectDto.Title,
                Description = createProjectDto.Description,
                ShortDescription = createProjectDto.ShortDescription,
                Repository = createProjectDto.Repository,
                DemoUrl = createProjectDto.DemoUrl,
                ImageUrl = imageUrl,
                Semester = createProjectDto.Semester,
                Year = createProjectDto.Year,
                TeamId = createProjectDto.TeamId,
                CreatedAt = DateTime.UtcNow
            };
            
            await _projectRepository.AddAsync(project);
            
            return await GetProjectByIdAsync(project.Id, userId);
        }
        
        public async Task<ProjectDto> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto, string userId)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            
            if (project == null)
                throw new Exception($"Project with ID {id} not found");
                
            // Check if user is allowed to update the project
            var isTeamMember = await _projectRepository.IsUserTeamMemberAsync(id, userId);
            var isAdmin = await _userRepository.IsUserInRoleAsync(userId, "Admin");
            
            if (!isTeamMember && !isAdmin)
                throw new Exception("You are not authorized to update this project");
                
            // Update properties if provided
            if (!string.IsNullOrEmpty(updateProjectDto.Title))
                project.Title = updateProjectDto.Title;
                
            if (!string.IsNullOrEmpty(updateProjectDto.Description))
                project.Description = updateProjectDto.Description;
                
            if (!string.IsNullOrEmpty(updateProjectDto.ShortDescription))
                project.ShortDescription = updateProjectDto.ShortDescription;
                
            if (!string.IsNullOrEmpty(updateProjectDto.Repository))
                project.Repository = updateProjectDto.Repository;
                
            if (!string.IsNullOrEmpty(updateProjectDto.DemoUrl))
                project.DemoUrl = updateProjectDto.DemoUrl;
                
            if (!string.IsNullOrEmpty(updateProjectDto.Semester))
                project.Semester = updateProjectDto.Semester;
                
            if (updateProjectDto.Year.HasValue)
                project.Year = updateProjectDto.Year.Value;
                
            if (updateProjectDto.TeamId.HasValue)
                project.TeamId = updateProjectDto.TeamId;
                
            // Update image if provided
            if (updateProjectDto.Image != null)
            {
                // Delete old image
                if (!string.IsNullOrEmpty(project.ImageUrl))
                {
                    await _storageService.DeleteFileAsync(project.ImageUrl);
                }
                
                // Upload new image
                project.ImageUrl = await _storageService.UploadFileAsync(updateProjectDto.Image, "projects");
            }
            
            project.UpdatedAt = DateTime.UtcNow;
            
            await _projectRepository.UpdateAsync(project);
            
            return await GetProjectByIdAsync(id, userId);
        }
        
        public async Task DeleteProjectAsync(int id, string userId)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            
            if (project == null)
                throw new Exception($"Project with ID {id} not found");
                
            // Check if user is allowed to delete the project
            var isTeamMember = await _projectRepository.IsUserTeamMemberAsync(id, userId);
            var isAdmin = await _userRepository.IsUserInRoleAsync(userId, "Admin");
            
            if (!isTeamMember && !isAdmin)
                throw new Exception("You are not authorized to delete this project");
                
            // Delete image
            if (!string.IsNullOrEmpty(project.ImageUrl))
            {
                await _storageService.DeleteFileAsync(project.ImageUrl);
            }
            
            await _projectRepository.DeleteAsync(project);
        }
        
        public async Task<bool> LikeProjectAsync(int id, string userId)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            
            if (project == null)
                throw new Exception($"Project with ID {id} not found");
                
            var hasLiked = await _projectRepository.HasUserLikedProjectAsync(id, userId);
            
            if (hasLiked)
                return false;
                
            await _projectRepository.AddLikeAsync(id, userId);
            return true;
        }
        
        public async Task<bool> UnlikeProjectAsync(int id, string userId)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            
            if (project == null)
                throw new Exception($"Project with ID {id} not found");
                
            var hasLiked = await _projectRepository.HasUserLikedProjectAsync(id, userId);
            
            if (!hasLiked)
                return false;
                
            await _projectRepository.RemoveLikeAsync(id, userId);
            return true;
        }
        
        public async Task<int> GetTotalProjectsCountAsync()
        {
            return await _projectRepository.GetTotalProjectsCountAsync();
        }
        
        // Helper methods
        private ProjectDto MapProjectToDto(Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                ShortDescription = project.ShortDescription,
                Repository = project.Repository,
                DemoUrl = project.DemoUrl,
                ImageUrl = project.ImageUrl,
                Semester = project.Semester,
                Year = project.Year,
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt,
                CommentsCount = project.Comments?.Count ?? 0,
                LikesCount = project.Likes?.Count ?? 0,
                Team = project.Team != null ? new TeamDto
                {
                    Id = project.Team.Id,
                    Name = project.Team.Name,
                    Members = project.Team.Members?.Select(tm => new TeamMemberDto
                    {
                        Id = tm.User.Id,
                        FirstName = tm.User.FirstName,
                        LastName = tm.User.LastName,
                        ProfilePicture = tm.User.ProfilePicture,
                        Role = tm.Role
                    }).ToList()
                } : null,
                Badges = project.Badges?.Select(pb => new BadgeDto
                {
                    Id = pb.Badge.Id,
                    Name = pb.Badge.Name,
                    ImageUrl = pb.Badge.ImageUrl,
                    AwardedAt = pb.AwardedAt
                }).ToList()
            };
        }
    }
}