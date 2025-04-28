using System.Collections.Generic;
using System.Threading.Tasks;
using CapstoneShowcase.Core.Entities;

namespace CapstoneShowcase.Core.Interfaces
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<Team> GetTeamByIdWithMembersAsync(int teamId);
        Task<IReadOnlyList<Team>> GetTeamsByUserIdAsync(string userId);
        Task<bool> IsUserTeamLeaderAsync(int teamId, string userId);
        Task<bool> IsUserTeamMemberAsync(int teamId, string userId);
        Task AddTeamMemberAsync(int teamId, string userId, string role);
        Task RemoveTeamMemberAsync(int teamId, string userId);
        Task UpdateTeamMemberRoleAsync(int teamId, string userId, string role);
    }
}
