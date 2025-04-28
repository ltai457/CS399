using CapstoneShowcase.Core.Entities;
using CapstoneShowcase.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneShowcase.Infrastructure.Data.Repositories
{
    public class ProjectBiddingRepository : IProjectBiddingRepository
    {
        private readonly AppDbContext _context;

        public ProjectBiddingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProjectBid>> GetBidsByStudentIdAsync(string studentId)
        {
            return await _context.ProjectBids
                .Include(pb => pb.Project)
                .Where(pb => pb.StudentId == studentId)
                .OrderBy(pb => pb.Preference)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<ProjectBid>> GetBidsByProjectIdAsync(int projectId)
        {
            return await _context.ProjectBids
                .Include(pb => pb.Student)
                    .ThenInclude(s => s.User)
                .Where(pb => pb.ProjectId == projectId)
                .OrderBy(pb => pb.Preference)
                .ToListAsync();
        }

        public async Task AddBidAsync(ProjectBid bid)
        {
            await _context.ProjectBids.AddAsync(bid);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBidAsync(ProjectBid bid)
        {
            var existingBid = await _context.ProjectBids
                .FirstOrDefaultAsync(pb => pb.StudentId == bid.StudentId && pb.ProjectId == bid.ProjectId);
                
            if (existingBid != null)
            {
                existingBid.Preference = bid.Preference;
                existingBid.Reason = bid.Reason;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveBidAsync(string studentId, int projectId)
        {
            var bid = await _context.ProjectBids
                .FirstOrDefaultAsync(pb => pb.StudentId == studentId && pb.ProjectId == projectId);
                
            if (bid != null)
            {
                _context.ProjectBids.Remove(bid);
                await _context.SaveChangesAsync();
            }
        } 
    }
}

