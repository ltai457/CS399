// CapstoneShowcase.Infrastructure/Services/BadgeService.cs
using CapstoneShowcase.Core.DTOs.Badges;
using CapstoneShowcase.Core.Entities;
using CapstoneShowcase.Core.Interfaces;
using CapstoneShowcase.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneShowcase.Infrastructure.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly IBadgeRepository _badgeRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStorageService _storageService;

        public BadgeService(
            IBadgeRepository badgeRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IStorageService storageService)
        {
            _badgeRepository = badgeRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _storageService = storageService;
        }

        public async Task<IEnumerable<BadgeDto>> GetAllBadgesAsync()
        {
            var badges = await _badgeRepository.GetAllBadgesAsync();
            
            return badges.Select(b => new BadgeDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                ImageUrl = b.ImageUrl
            });
        }

        public async Task<BadgeDto> CreateBadgeAsync(CreateBadgeDto createBadgeDto, string userId)
        {
            // Check if user is admin
            var isAdmin = await _userRepository.IsUserInRoleAsync(userId, "Admin");
            if (!isAdmin)
                throw new Exception("Only administrators can create badges");

            // Upload image
            string imageUrl = null;
            if (createBadgeDto.Image != null)
            {
                imageUrl = await _storageService.UploadFileAsync(createBadgeDto.Image, "badges");
            }

            // Create badge
            var badge = new Badge
            {
                Name = createBadgeDto.Name,
                Description = createBadgeDto.Description,
                ImageUrl = imageUrl
            };

            await _badgeRepository.AddAsync(badge);

            // Return DTO
            return new BadgeDto
            {
                Id = badge.Id,
                Name = badge.Name,
                Description = badge.Description,
                ImageUrl = badge.ImageUrl
            };
        }

        public async Task AwardBadgeToProjectAsync(int projectId, int badgeId, string awardedByUserId)
        {
            // Check if user is admin
            var isAdmin = await _userRepository.IsUserInRoleAsync(awardedByUserId, "Admin");
            if (!isAdmin)
                throw new Exception("Only administrators can award badges");

            // Check if project and badge exist
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new Exception($"Project with ID {projectId} not found");

            var badge = await _badgeRepository.GetByIdAsync(badgeId);
            if (badge == null)
                throw new Exception($"Badge with ID {badgeId} not found");

            // Award badge
            await _badgeRepository.AddBadgeToProjectAsync(projectId, badgeId, awardedByUserId);
        }

        public async Task RemoveBadgeFromProjectAsync(int projectId, int badgeId, string userId)
        {
            // Check if user is admin
            var isAdmin = await _userRepository.IsUserInRoleAsync(userId, "Admin");
            if (!isAdmin)
                throw new Exception("Only administrators can remove badges");

            // Check if project and badge exist
            var project = await _projectRepository.GetByIdAsync(projectId);
            if (project == null)
                throw new Exception($"Project with ID {projectId} not found");

            var badge = await _badgeRepository.GetByIdAsync(badgeId);
            if (badge == null)
                throw new Exception($"Badge with ID {badgeId} not found");

            // Remove badge
            await _badgeRepository.RemoveBadgeFromProjectAsync(projectId, badgeId);
        }
    }
}