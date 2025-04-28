using CapstoneShowcase.Core.Entities;
using CapstoneShowcase.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneShowcase.Infrastructure.Data.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Project>> GetProjectsWithDetailsAsync(int skip = 0, int take = 10)
        {
            return await _context.Projects
                .Include(p => p.Team)
                    .ThenInclude(t => t.Members)
                        .ThenInclude(tm => tm.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .Include(p => p.Badges)
                    .ThenInclude(pb => pb.Badge)
                .OrderByDescending(p => p.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<Project> GetProjectByIdWithDetailsAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Team)
                    .ThenInclude(t => t.Members)
                        .ThenInclude(tm => tm.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Badges)
                    .ThenInclude(pb => pb.Badge)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> GetTotalProjectsCountAsync()
        {
            return await _context.Projects.CountAsync();
        }

        public async Task<bool> IsUserTeamMemberAsync(int projectId, string userId)
        {
            var project = await _context.Projects
                .Include(p => p.Team)
                    .ThenInclude(t => t.Members)
                .FirstOrDefaultAsync(p => p.Id == projectId);
                
            if (project?.Team == null)
                return false;
                
            return project.Team.Members.Any(tm => tm.UserId == userId);
        }

        public async Task<int> GetLikesCountAsync(int projectId)
        {
            return await _context.Likes.CountAsync(l => l.ProjectId == projectId);
        }

        public async Task<bool> HasUserLikedProjectAsync(int projectId, string userId)
        {
            return await _context.Likes
                .AnyAsync(l => l.ProjectId == projectId && l.UserId == userId);
        }

        public async Task AddLikeAsync(int projectId, string userId)
        {
            var like = new Like
            {
                ProjectId = projectId,
                UserId = userId
            };
            
            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveLikeAsync(int projectId, string userId)
        {
            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.ProjectId == projectId && l.UserId == userId);
                
            if (like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
            }
        }
    }
}