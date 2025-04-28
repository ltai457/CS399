using CapstoneShowcase.Core.DTOs.Badges;
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
    public class BadgesController : ControllerBase
    {
        private readonly IBadgeService _badgeService;
        
        public BadgesController(IBadgeService badgeService)
        {
            _badgeService = badgeService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetBadges()
        {
            try
            {
                var badges = await _badgeService.GetAllBadgesAsync();
                return Ok(badges);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBadge([FromForm] CreateBadgeDto createBadgeDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var badge = await _badgeService.CreateBadgeAsync(createBadgeDto, userId);
                return CreatedAtAction(nameof(GetBadges), null, badge);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost("projects/{projectId}/award/{badgeId}")]
        public async Task<IActionResult> AwardBadgeToProject(int projectId, int badgeId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _badgeService.AwardBadgeToProjectAsync(projectId, badgeId, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [Authorize(Roles = "Admin")]
        [HttpDelete("projects/{projectId}/award/{badgeId}")]
        public async Task<IActionResult> RemoveBadgeFromProject(int projectId, int badgeId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _badgeService.RemoveBadgeFromProjectAsync(projectId, badgeId, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}