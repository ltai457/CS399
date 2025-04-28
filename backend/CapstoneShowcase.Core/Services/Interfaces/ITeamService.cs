using CapstoneShowcase.Core.DTOs.Teams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapstoneShowcase.Core.Services.Interfaces
{
    public interface ITeamService
    {
        Task<IEnumerable<TeamDto>> GetTeamsAsync();
        Task<TeamDto> GetTeamByIdAsync(int id);
        Task<IEnumerable<TeamDto>> GetUserTeamsAsync(string userId);
        Task<TeamDto> CreateTeamAsync(CreateTeamDto createTeamDto, string userId);
        Task<TeamDto> UpdateTeamAsync(int id, CreateTeamDto updateTeamDto, string userId);
        Task DeleteTeamAsync(int id, string userId);
        Task AddTeamMemberAsync(int teamId, string memberId, string role, string userId);
        Task RemoveTeamMemberAsync(int teamId, string memberId, string userId);
        Task UpdateTeamMemberRoleAsync(int teamId, string memberId, string role, string userId);
    }
}