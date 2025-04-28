using System.Collections.Generic;
using System.Threading.Tasks;
using CapstoneShowcase.Core.Entities;

namespace CapstoneShowcase.Core.Interfaces
{
    public interface ITeamFormationRepository
    {
        Task<TeamFormationProfile> GetProfileByStudentIdAsync(string studentId);
        Task<IReadOnlyList<TeamFormationProfile>> GetAllProfilesAsync();
        Task<IReadOnlyList<TeamFormationProfile>> GetProfilesByInterestsAsync(IEnumerable<string> interests);
        Task<IReadOnlyList<TeamFormationProfile>> GetProfilesBySkillsAsync(IEnumerable<string> skills);
        Task CreateOrUpdateProfileAsync(TeamFormationProfile profile);
    }
}