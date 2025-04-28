using System.Collections.Generic;
using System.Threading.Tasks;
using CapstoneShowcase.Core.Entities;

namespace CapstoneShowcase.Core.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<IReadOnlyList<Project>> GetProjectsWithDetailsAsync(int skip = 0, int take = 10);
        Task<Project> GetProjectByIdWithDetailsAsync(int id);
        Task<int> GetTotalProjectsCountAsync();
        Task<bool> IsUserTeamMemberAsync(int projectId, string userId);
        Task<int> GetLikesCountAsync(int projectId);
        Task<bool> HasUserLikedProjectAsync(int projectId, string userId);
        Task AddLikeAsync(int projectId, string userId);
        Task RemoveLikeAsync(int projectId, string userId);
    }
}
