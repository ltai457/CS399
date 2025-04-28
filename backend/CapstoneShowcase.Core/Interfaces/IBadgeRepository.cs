using System.Collections.Generic;
using System.Threading.Tasks;
using CapstoneShowcase.Core.Entities;

namespace CapstoneShowcase.Core.Interfaces
{
    public interface IBadgeRepository : IRepository<Badge>
    {
        Task<IReadOnlyList<Badge>> GetAllBadgesAsync();
        Task<IReadOnlyList<ProjectBadge>> GetBadgesByProjectIdAsync(int projectId);
        Task AddBadgeToProjectAsync(int projectId, int badgeId, string awardedByUserId);
        Task RemoveBadgeFromProjectAsync(int projectId, int badgeId);
    }
}