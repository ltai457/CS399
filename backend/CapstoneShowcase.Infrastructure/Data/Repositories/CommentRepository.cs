using CapstoneShowcase.Core.Entities;
using CapstoneShowcase.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneShowcase.Infrastructure.Data.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Comment>> GetCommentsByProjectIdAsync(int projectId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.ProjectId == projectId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> IsUserCommentOwnerAsync(int commentId, string userId)
        {
            return await _context.Comments
                .AnyAsync(c => c.Id == commentId && c.UserId == userId);
        }
    }
}