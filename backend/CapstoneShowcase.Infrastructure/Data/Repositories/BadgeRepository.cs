using CapstoneShowcase.Core.Entities;
using CapstoneShowcase.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneShowcase.Infrastructure.Data.Repositories
{
    public class BadgeRepository : Repository<Badge>, IBadgeRepository
    {
        public BadgeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Badge>> GetAllBadgesAsync()
        {
            return await _context.Badges.ToListAsync();
        }

        public async Task<IReadOnlyList<ProjectBadge>> GetBadgesByProjectIdAsync(int projectId)
        {
            return await _context.ProjectBadges
                .Include(pb => pb.Badge)
                .Include(pb => pb.AwardedBy)
                .Where(pb => pb.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task AddBadgeToProjectAsync(int projectId, int badgeId, string awardedByUserId)
        {
            var projectBadge = new ProjectBadge
            {
                ProjectId = projectId,
                BadgeId = badgeId,
                AwardedByUserId = awardedByUserId,
                AwardedAt = DateTime.UtcNow
            };
            
            await _context.ProjectBadges.AddAsync(projectBadge);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveBadgeFromProjectAsync(int projectId, int badgeId)
        {
            var projectBadge = await _context.ProjectBadges
                .FirstOrDefaultAsync(pb => pb.ProjectId == projectId && pb.BadgeId == badgeId);
                
            if (projectBadge != null)
            {
                _context.ProjectBadges.Remove(projectBadge);
                await _context.SaveChangesAsync();
            }
        }
    }
}