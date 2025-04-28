using System.Collections.Generic;
using System.Threading.Tasks;
using CapstoneShowcase.Core.Entities;

namespace CapstoneShowcase.Core.Interfaces
{
    public interface IProjectBiddingRepository
    {
        Task<IReadOnlyList<ProjectBid>> GetBidsByStudentIdAsync(string studentId);
        Task<IReadOnlyList<ProjectBid>> GetBidsByProjectIdAsync(int projectId);
        Task AddBidAsync(ProjectBid bid);
        Task UpdateBidAsync(ProjectBid bid);
        Task RemoveBidAsync(string studentId, int projectId);
    }
}