using CapstoneShowcase.Core.DTOs.TeamFormation;
using CapstoneShowcase.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CapstoneShowcase.API.Controllers
{
    [Route("api/team-formation")]
    [ApiController]
    public class TeamFormationController : ControllerBase
    {
        private readonly ITeamFormationService _teamFormationService;
        
        public TeamFormationController(ITeamFormationService teamFormationService)
        {
            _teamFormationService = teamFormationService;
        }
        
        [Authorize(Roles = "Student")]
        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var profile = await _teamFormationService.GetProfileAsync(userId);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Roles = "Student")]
        [HttpGet("students")]
        public async Task<IActionResult> GetAllProfiles()
        {
            try
            {
                var profiles = await _teamFormationService.GetAllProfilesAsync();
                return Ok(profiles);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Roles = "Student")]
        [HttpGet("by-interests")]
        public async Task<IActionResult> FindTeammatesByInterests([FromQuery] string[] interests)
        {
            try
            {
                var profiles = await _teamFormationService.FindTeammatesByInterestsAsync(interests);
                return Ok(profiles);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Roles = "Student")]
        [HttpGet("by-skills")]
        public async Task<IActionResult> FindTeammatesBySkills([FromQuery] string[] skills)
        {
            try
            {
                var profiles = await _teamFormationService.FindTeammatesBySkillsAsync(skills);
                return Ok(profiles);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Roles = "Student")]
        [HttpPost("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateTeamFormationProfileDto profileDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _teamFormationService.CreateOrUpdateProfileAsync(userId, profileDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}