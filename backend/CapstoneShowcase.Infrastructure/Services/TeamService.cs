// CapstoneShowcase.Infrastructure/Services/TeamService.cs
using CapstoneShowcase.Core.DTOs.Teams;
using CapstoneShowcase.Core.Entities;
using CapstoneShowcase.Core.Interfaces;
using CapstoneShowcase.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneShowcase.Infrastructure.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public TeamService(
            ITeamRepository teamRepository,
            IUserRepository userRepository,
            UserManager<User> userManager)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<TeamDto>> GetTeamsAsync()
        {
            var teams = await _teamRepository.GetAllAsync();
            var result = new List<TeamDto>();

            foreach (var team in teams)
            {
                var teamWithMembers = await _teamRepository.GetTeamByIdWithMembersAsync(team.Id);
                result.Add(MapTeamToDto(teamWithMembers));
            }

            return result;
        }

        public async Task<TeamDto> GetTeamByIdAsync(int id)
        {
            var team = await _teamRepository.GetTeamByIdWithMembersAsync(id);
            if (team == null)
                throw new Exception($"Team with ID {id} not found");

            return MapTeamToDto(team);
        }

        public async Task<IEnumerable<TeamDto>> GetUserTeamsAsync(string userId)
        {
            var teams = await _teamRepository.GetTeamsByUserIdAsync(userId);
            return teams.Select(MapTeamToDto);
        }

        public async Task<TeamDto> CreateTeamAsync(CreateTeamDto createTeamDto, string userId)
        {
            // Check if user exists
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            // Create team
            var team = new Team
            {
                Name = createTeamDto.Name,
                Description = createTeamDto.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _teamRepository.AddAsync(team);

            // Add creator as team leader
            await _teamRepository.AddTeamMemberAsync(team.Id, userId, "Leader");

            // Add other members if specified
            if (createTeamDto.MemberIds != null && createTeamDto.MemberIds.Count > 0)
            {
                foreach (var memberId in createTeamDto.MemberIds)
                {
                    // Check if user exists
                    var member = await _userManager.FindByIdAsync(memberId);
                    if (member == null)
                        continue;

                    // Add as member
                    await _teamRepository.AddTeamMemberAsync(team.Id, memberId, "Member");
                }
            }

            // Return team with members
            return await GetTeamByIdAsync(team.Id);
        }

        public async Task<TeamDto> UpdateTeamAsync(int id, CreateTeamDto updateTeamDto, string userId)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null)
                throw new Exception($"Team with ID {id} not found");

            // Check if user is team leader
            var isLeader = await _teamRepository.IsUserTeamLeaderAsync(id, userId);
            var isAdmin = await _userRepository.IsUserInRoleAsync(userId, "Admin");

            if (!isLeader && !isAdmin)
                throw new Exception("You are not authorized to update this team");

            // Update properties
            team.Name = updateTeamDto.Name;
            team.Description = updateTeamDto.Description;
            team.UpdatedAt = DateTime.UtcNow;

            await _teamRepository.UpdateAsync(team);

            // Get current members
            var teamWithMembers = await _teamRepository.GetTeamByIdWithMembersAsync(id);
            var currentMemberIds = teamWithMembers.Members.Select(m => m.UserId).ToList();
            var leaderIds = teamWithMembers.Members.Where(m => m.Role == "Leader").Select(m => m.UserId).ToList();

            // Add new members
            if (updateTeamDto.MemberIds != null)
            {
                foreach (var memberId in updateTeamDto.MemberIds)
                {
                    if (!currentMemberIds.Contains(memberId))
                    {
                        // Check if user exists
                        var member = await _userManager.FindByIdAsync(memberId);
                        if (member == null)
                            continue;

                        // Add as member
                        await _teamRepository.AddTeamMemberAsync(id, memberId, "Member");
                    }
                }

                // Remove members not in the updated list (except leaders)
                foreach (var currentMemberId in currentMemberIds)
                {
                    if (!updateTeamDto.MemberIds.Contains(currentMemberId) && !leaderIds.Contains(currentMemberId))
                    {
                        await _teamRepository.RemoveTeamMemberAsync(id, currentMemberId);
                    }
                }
            }

            // Return updated team
            return await GetTeamByIdAsync(id);
        }

        public async Task DeleteTeamAsync(int id, string userId)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null)
                throw new Exception($"Team with ID {id} not found");

            // Check if user is team leader or admin
            var isLeader = await _teamRepository.IsUserTeamLeaderAsync(id, userId);
            var isAdmin = await _userRepository.IsUserInRoleAsync(userId, "Admin");

            if (!isLeader && !isAdmin)
                throw new Exception("You are not authorized to delete this team");

            await _teamRepository.DeleteAsync(team);
        }

        public async Task AddTeamMemberAsync(int teamId, string memberId, string role, string userId)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team == null)
                throw new Exception($"Team with ID {teamId} not found");

            // Check if user is team leader or admin
            var isLeader = await _teamRepository.IsUserTeamLeaderAsync(teamId, userId);
            var isAdmin = await _userRepository.IsUserInRoleAsync(userId, "Admin");

            if (!isLeader && !isAdmin)
                throw new Exception("You are not authorized to add members to this team");

            // Check if the member already exists in the team
            var isMember = await _teamRepository.IsUserTeamMemberAsync(teamId, memberId);
            if (isMember)
                throw new Exception("User is already a member of this team");

            // Check if the member exists
            var member = await _userManager.FindByIdAsync(memberId);
            if (member == null)
                throw new Exception("User not found");

            // Add the member
            await _teamRepository.AddTeamMemberAsync(teamId, memberId, role);
        }

        public async Task RemoveTeamMemberAsync(int teamId, string memberId, string userId)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team == null)
                throw new Exception($"Team with ID {teamId} not found");

            // Check if user is team leader or admin
            var isLeader = await _teamRepository.IsUserTeamLeaderAsync(teamId, userId);
            var isAdmin = await _userRepository.IsUserInRoleAsync(userId, "Admin");

            if (!isLeader && !isAdmin)
                throw new Exception("You are not authorized to remove members from this team");

            // Cannot remove the last leader
            var teamWithMembers = await _teamRepository.GetTeamByIdWithMembersAsync(teamId);
            var leaders = teamWithMembers.Members.Where(m => m.Role == "Leader").ToList();
            
            if (leaders.Count == 1 && leaders[0].UserId == memberId)
                throw new Exception("Cannot remove the last team leader. Assign another leader first.");

            // Remove the member
            await _teamRepository.RemoveTeamMemberAsync(teamId, memberId);
        }

        public async Task UpdateTeamMemberRoleAsync(int teamId, string memberId, string role, string userId)
        {
            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team == null)
                throw new Exception($"Team with ID {teamId} not found");

            // Check if user is team leader or admin
            var isLeader = await _teamRepository.IsUserTeamLeaderAsync(teamId, userId);
            var isAdmin = await _userRepository.IsUserInRoleAsync(userId, "Admin");

            if (!isLeader && !isAdmin)
                throw new Exception("You are not authorized to update member roles in this team");

            // Check if the member exists in the team
            var isMember = await _teamRepository.IsUserTeamMemberAsync(teamId, memberId);
            if (!isMember)
                throw new Exception("User is not a member of this team");

            // Update the role
            await _teamRepository.UpdateTeamMemberRoleAsync(teamId, memberId, role);
        }

        // Helper methods
        private TeamDto MapTeamToDto(Team team)
        {
            return new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description,
                CreatedAt = team.CreatedAt,
                Members = team.Members?.Select(m => new TeamMemberDto
                {
                    Id = m.UserId,
                    FirstName = m.User.FirstName,
                    LastName = m.User.LastName,
                    ProfilePicture = m.User.ProfilePicture,
                    Role = m.Role
                }).ToList() ?? new List<TeamMemberDto>()
            };
        }
    }
}