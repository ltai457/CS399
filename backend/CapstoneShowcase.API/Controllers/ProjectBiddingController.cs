using CapstoneShowcase.Core.DTOs.ProjectBidding;
using CapstoneShowcase.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CapstoneShowcase.API.Controllers
{
    [Route("api/project-bidding")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class ProjectBiddingController : ControllerBase
    {
        private readonly IProjectBiddingService _projectBiddingService;
        
        public ProjectBiddingController(IProjectBiddingService projectBiddingService)
        {
            _projectBiddingService = projectBiddingService;
        }
        
        [HttpGet("my-bids")]
        public async Task<IActionResult> GetMyBids()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var bids = await _projectBiddingService.GetStudentBidsAsync(userId);
                return Ok(bids);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("projects/{projectId}/bids")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProjectBids(int projectId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var bids = await _projectBiddingService.GetProjectBidsAsync(projectId, userId);
                return Ok(bids);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPost("bid")]
        public async Task<IActionResult> CreateBid([FromBody] CreateProjectBidDto bidDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _projectBiddingService.CreateBidAsync(userId, bidDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPut("bid")]
        public async Task<IActionResult> UpdateBid([FromBody] CreateProjectBidDto bidDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _projectBiddingService.UpdateBidAsync(userId, bidDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpDelete("bid/{projectId}")]
        public async Task<IActionResult> DeleteBid(int projectId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _projectBiddingService.DeleteBidAsync(userId, projectId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}