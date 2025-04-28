using CapstoneShowcase.Core.DTOs.ProjectBidding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapstoneShowcase.Core.Services.Interfaces
{
    public interface IProjectBiddingService
    {
        Task<IEnumerable<ProjectBidDto>> GetStudentBidsAsync(string studentId);
        Task<IEnumerable<ProjectBidDto>> GetProjectBidsAsync(int projectId, string userId);
        Task CreateBidAsync(string studentId, CreateProjectBidDto bidDto);
        Task UpdateBidAsync(string studentId, CreateProjectBidDto bidDto);
        Task DeleteBidAsync(string studentId, int projectId);
    }
}