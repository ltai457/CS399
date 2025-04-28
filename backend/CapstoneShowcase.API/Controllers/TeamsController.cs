using CapstoneShowcase.Core.DTOs.Teams;
using CapstoneShowcase.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CapstoneShowcase.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        
        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetTeams()
        {
            try
            {
                var teams = await _teamService.GetTeamsAsync();
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamById(int id)
        {
            try
            {
                var team = await _teamService.GetTeamByIdAsync(id);
                return Ok(team);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        
        [Authorize]
        [HttpGet("my-teams")]
        public async Task<IActionResult> GetMyTeams()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var teams = await _teamService.GetUserTeamsAsync(userId);
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Roles = "Student")]
        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] CreateTeamDto createTeamDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var team = await _teamService.CreateTeamAsync(createTeamDto, userId);
                return CreatedAtAction(nameof(GetTeamById), new { id = team.Id }, team);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Roles = "Student")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, [FromBody] CreateTeamDto updateTeamDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var team = await _teamService.UpdateTeamAsync(id, updateTeamDto, userId);
                return Ok(team);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Roles = "Student,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _teamService.DeleteTeamAsync(id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Roles = "Student")]
        [HttpPost("{teamId}/members/{memberId}")]
        public async Task<IActionResult> AddTeamMember(int teamId, string memberId, [FromQuery] string role = "Member")
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _teamService.AddTeamMemberAsync(teamId, memberId, role, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Roles = "Student")]
        [HttpDelete("{teamId}/members/{memberId}")]
        public async Task<IActionResult> RemoveTeamMember(int teamId, string memberId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _teamService.RemoveTeamMemberAsync(teamId, memberId, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Roles = "Student")]
        [HttpPut("{teamId}/members/{memberId}/role")]
        public async Task<IActionResult> UpdateTeamMemberRole(int teamId, string memberId, [FromQuery] string role)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _teamService.UpdateTeamMemberRoleAsync(teamId, memberId, role, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
