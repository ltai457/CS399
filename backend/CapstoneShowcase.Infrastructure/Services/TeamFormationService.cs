// CapstoneShowcase.Infrastructure/Services/TeamFormationService.cs
using CapstoneShowcase.Core.DTOs.TeamFormation;
using CapstoneShowcase.Core.Entities;
using CapstoneShowcase.Core.Interfaces;
using CapstoneShowcase.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneShowcase.Infrastructure.Services
{
    public class TeamFormationService : ITeamFormationService
    {
        private readonly ITeamFormationRepository _teamFormationRepository;
        private readonly IUserRepository _userRepository;

        public TeamFormationService(
            ITeamFormationRepository teamFormationRepository,
            IUserRepository userRepository)
        {
            _teamFormationRepository = teamFormationRepository;
            _userRepository = userRepository;
        }

        public async Task<TeamFormationProfileDto> GetProfileAsync(string studentId)
        {
            var profile = await _teamFormationRepository.GetProfileByStudentIdAsync(studentId);
            
            if (profile == null)
                return null;

            return MapProfileToDto(profile);
        }

        public async Task<IEnumerable<TeamFormationProfileDto>> GetAllProfilesAsync()
        {
            var profiles = await _teamFormationRepository.GetAllProfilesAsync();
            return profiles.Select(MapProfileToDto);
        }

        public async Task<IEnumerable<TeamFormationProfileDto>> FindTeammatesByInterestsAsync(IEnumerable<string> interests)
        {
            if (interests == null || !interests.Any())
                return await GetAllProfilesAsync();

            var profiles = await _teamFormationRepository.GetProfilesByInterestsAsync(interests);
            return profiles.Select(MapProfileToDto);
        }

        public async Task<IEnumerable<TeamFormationProfileDto>> FindTeammatesBySkillsAsync(IEnumerable<string> skills)
        {
            if (skills == null || !skills.Any())
                return await GetAllProfilesAsync();

            var profiles = await _teamFormationRepository.GetProfilesBySkillsAsync(skills);
            return profiles.Select(MapProfileToDto);
        }

        public async Task CreateOrUpdateProfileAsync(string studentId, UpdateTeamFormationProfileDto profileDto)
        {
            // Check if student exists
            var student = await _userRepository.GetStudentByIdAsync(studentId);
            if (student == null)
                throw new Exception("Student not found");

            // Create or update the profile
            var profile = await _teamFormationRepository.GetProfileByStudentIdAsync(studentId);
            
            if (profile == null)
            {
                profile = new TeamFormationProfile
                {
                    StudentId = studentId,
                    Skills = profileDto.Skills ?? new List<string>(),
                    Interests = profileDto.Interests ?? new List<string>(),
                    LookingForTeam = profileDto.LookingForTeam,
                    CreatedAt = DateTime.UtcNow
                };
            }
            else
            {
                profile.Skills = profileDto.Skills ?? profile.Skills;
                profile.Interests = profileDto.Interests ?? profile.Interests;
                profile.LookingForTeam = profileDto.LookingForTeam;
                profile.UpdatedAt = DateTime.UtcNow;
            }

            await _teamFormationRepository.CreateOrUpdateProfileAsync(profile);
        }

        // Helper methods
        private TeamFormationProfileDto MapProfileToDto(TeamFormationProfile profile)
        {
            if (profile == null || profile.Student == null || profile.Student.User == null)
                return null;

            return new TeamFormationProfileDto
            {
                StudentId = profile.StudentId,
                FirstName = profile.Student.User.FirstName,
                LastName = profile.Student.User.LastName,
                Email = profile.Student.User.Email,
                ProfilePicture = profile.Student.User.ProfilePicture,
                Skills = profile.Skills,
                Interests = profile.Interests,
                GithubProfile = profile.Student.GithubProfile,
                LinkedInProfile = profile.Student.LinkedInProfile,
                Biography = profile.Student.Biography,
                LookingForTeam = profile.LookingForTeam
            };
        }
    }
}