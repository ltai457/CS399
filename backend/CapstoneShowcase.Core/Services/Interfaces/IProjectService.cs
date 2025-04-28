using CapstoneShowcase.Core.DTOs.Projects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapstoneShowcase.Core.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetProjectsAsync(int page = 1, int pageSize = 10);
        Task<ProjectDto> GetProjectByIdAsync(int id, string currentUserId = null);
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto createProjectDto, string userId);
        Task<ProjectDto> UpdateProjectAsync(int id, UpdateProjectDto updateProjectDto, string userId);
        Task DeleteProjectAsync(int id, string userId);
        Task<bool> LikeProjectAsync(int id, string userId);
        Task<bool> UnlikeProjectAsync(int id, string userId);
        Task<int> GetTotalProjectsCountAsync();
    }
}