using CapstoneShowcase.Core.Entities;
using CapstoneShowcase.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneShowcase.Infrastructure.Data.Repositories
{
    public class TeamRepository : Repository<Team>, ITeamRepository
    {
        public TeamRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Team> GetTeamByIdWithMembersAsync(int teamId)
        {
            return await _context.Teams
                .Include(t => t.Members)
                    .ThenInclude(tm => tm.User)
                .FirstOrDefaultAsync(t => t.Id == teamId);
        }

        public async Task<IReadOnlyList<Team>> GetTeamsByUserIdAsync(string userId)
        {
            return await _context.TeamMembers
                .Include(tm => tm.Team)
                    .ThenInclude(t => t.Members)
                        .ThenInclude(tm => tm.User)
                .Where(tm => tm.UserId == userId)
                .Select(tm => tm.Team)
                .ToListAsync();
        }

        public async Task<bool> IsUserTeamLeaderAsync(int teamId, string userId)
        {
            return await _context.TeamMembers
                .AnyAsync(tm => tm.TeamId == teamId && tm.UserId == userId && tm.Role == "Leader");
        }

        public async Task<bool> IsUserTeamMemberAsync(int teamId, string userId)
        {
            return await _context.TeamMembers
                .AnyAsync(tm => tm.TeamId == teamId && tm.UserId == userId);
        }

        public async Task AddTeamMemberAsync(int teamId, string userId, string role)
        {
            var teamMember = new TeamMember
            {
                TeamId = teamId,
                UserId = userId,
                Role = role
            };
            
            await _context.TeamMembers.AddAsync(teamMember);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTeamMemberAsync(int teamId, string userId)
        {
            var teamMember = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId);
                
            if (teamMember != null)
            {
                _context.TeamMembers.Remove(teamMember);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateTeamMemberRoleAsync(int teamId, string userId, string role)
        {
            var teamMember = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.TeamId == teamId && tm.UserId == userId);
                
            if (teamMember != null)
            {
                teamMember.Role = role;
                await _context.SaveChangesAsync();
            }
        }
    }
}