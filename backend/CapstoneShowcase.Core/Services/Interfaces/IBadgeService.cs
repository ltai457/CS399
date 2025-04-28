using CapstoneShowcase.Core.DTOs.Badges;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapstoneShowcase.Core.Services.Interfaces
{
    public interface IBadgeService
    {
        Task<IEnumerable<BadgeDto>> GetAllBadgesAsync();
        Task<BadgeDto> CreateBadgeAsync(CreateBadgeDto createBadgeDto, string userId);
        Task AwardBadgeToProjectAsync(int projectId, int badgeId, string awardedByUserId);
        Task RemoveBadgeFromProjectAsync(int projectId, int badgeId, string userId);
    }
}