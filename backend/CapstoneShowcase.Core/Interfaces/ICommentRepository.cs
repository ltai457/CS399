using System.Collections.Generic;
using System.Threading.Tasks;
using CapstoneShowcase.Core.Entities;

namespace CapstoneShowcase.Core.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IReadOnlyList<Comment>> GetCommentsByProjectIdAsync(int projectId);
        Task<bool> IsUserCommentOwnerAsync(int commentId, string userId);
    }
}