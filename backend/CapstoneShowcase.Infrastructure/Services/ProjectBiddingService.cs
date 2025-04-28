// CapstoneShowcase.Infrastructure/Services/ProjectBiddingService.cs
using CapstoneShowcase.Core.DTOs.ProjectBidding;
using CapstoneShowcase.Core.Entities;
using CapstoneShowcase.Core.Interfaces;
using CapstoneShowcase.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneShowcase.Infrastructure.Services
{
    public class ProjectBiddingService : IProjectBiddingService
    {
        private readonly IProjectBiddingRepository _projectBiddingRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public ProjectBiddingService(
            IProjectBiddingRepository projectBiddingRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository)
        {
            _projectBiddingRepository = projectBiddingRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ProjectBidDto>> GetStudentBidsAsync(string studentId)
        {
            // Check if student exists
            var student = await _userRepository.GetStudentByIdAsync(studentId);
            if (student == null)
                throw new Exception("Student not found");

            var bids = await _projectBiddingRepository.GetBidsByStudentIdAsync(studentId);
            
            return bids.Select(b => new ProjectBidDto
            {
                ProjectId = b.ProjectId,
                ProjectTitle = b.Project?.Title,
                ProjectShortDescription = b.Project?.ShortDescription,
                Preference = b.Preference,
                Reason = b.Reason
            });
        }

        public async Task<IEnumerable<ProjectBidDto>> GetProjectBidsAsync(int projectId, string userId)
        {
            // Check if project exists
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new Exception($"Project with ID {projectId} not found");
                
            // Check if user is admin
            var isAdmin = await _userRepository.IsUserInRoleAsync(userId, "Admin");
            if (!isAdmin)
                throw new Exception("Only administrators can view all bids for a project");

            var bids = await _projectBiddingRepository.GetBidsByProjectIdAsync(projectId);
            
            return bids.Select(b => new ProjectBidDto
            {
                ProjectId = b.ProjectId,
                ProjectTitle = project.Title,
                ProjectShortDescription = project.ShortDescription,
                Preference = b.Preference,
                Reason = b.Reason
            });
        }

        public async Task CreateBidAsync(string studentId, CreateProjectBidDto bidDto)
        {
            // Check if student exists
            var student = await _userRepository.GetStudentByIdAsync(studentId);
            if (student == null)
                throw new Exception("Student not found");

            // Check if project exists
            var project = await _projectRepository.GetByIdAsync(bidDto.ProjectId);
            if (project == null)
                throw new Exception($"Project with ID {bidDto.ProjectId} not found");

            // Check if bid already exists
            var existingBids = await _projectBiddingRepository.GetBidsByStudentIdAsync(studentId);
            var existingBid = existingBids.FirstOrDefault(b => b.ProjectId == bidDto.ProjectId);
            
            if (existingBid != null)
                throw new Exception("You already have a bid for this project. Use update instead.");

            // Check if another bid with the same preference exists
            var duplicatePreference = existingBids.FirstOrDefault(b => b.Preference == bidDto.Preference);
            if (duplicatePreference != null)
                throw new Exception($"You already have a bid with preference {bidDto.Preference}. Please use a different preference value.");

            // Create bid
            var bid = new ProjectBid
            {
                StudentId = studentId,
                ProjectId = bidDto.ProjectId,
                Preference = bidDto.Preference,
                Reason = bidDto.Reason
            };

            await _projectBiddingRepository.AddBidAsync(bid);
        }

        public async Task UpdateBidAsync(string studentId, CreateProjectBidDto bidDto)
        {
            // Check if student exists
            var student = await _userRepository.GetStudentByIdAsync(studentId);
            if (student == null)
                throw new Exception("Student not found");

            // Check if project exists
            var project = await _projectRepository.GetByIdAsync(bidDto.ProjectId);
            if (project == null)
                throw new Exception($"Project with ID {bidDto.ProjectId} not found");

            // Check if bid exists
            var existingBids = await _projectBiddingRepository.GetBidsByStudentIdAsync(studentId);
            var existingBid = existingBids.FirstOrDefault(b => b.ProjectId == bidDto.ProjectId);
            
            if (existingBid == null)
                throw new Exception("Bid not found. Create a new bid instead.");

            // Check if another bid with the same preference exists (except the one being updated)
            var duplicatePreference = existingBids.FirstOrDefault(b => 
                b.Preference == bidDto.Preference && b.ProjectId != bidDto.ProjectId);
                
            if (duplicatePreference != null)
                throw new Exception($"You already have another bid with preference {bidDto.Preference}. Please use a different preference value.");

            // Update bid
            existingBid.Preference = bidDto.Preference;
            existingBid.Reason = bidDto.Reason;

            await _projectBiddingRepository.UpdateBidAsync(existingBid);
        }

        public async Task DeleteBidAsync(string studentId, int projectId)
        {
            // Check if student exists
            var student = await _userRepository.GetStudentByIdAsync(studentId);
            if (student == null)
                throw new Exception("Student not found");

            // Check if project exists
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new Exception($"Project with ID {projectId} not found");

            // Delete bid
            await _projectBiddingRepository.RemoveBidAsync(studentId, projectId);
        }
    }
}