using CapstoneShowcase.Core.DTOs.TeamFormation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapstoneShowcase.Core.Services.Interfaces
{
    public interface ITeamFormationService
    {
        Task<TeamFormationProfileDto> GetProfileAsync(string studentId);
        Task<IEnumerable<TeamFormationProfileDto>> GetAllProfilesAsync();
        Task<IEnumerable<TeamFormationProfileDto>> FindTeammatesByInterestsAsync(IEnumerable<string> interests);
        Task<IEnumerable<TeamFormationProfileDto>> FindTeammatesBySkillsAsync(IEnumerable<string> skills);
        Task CreateOrUpdateProfileAsync(string studentId, UpdateTeamFormationProfileDto profileDto);
    }
}