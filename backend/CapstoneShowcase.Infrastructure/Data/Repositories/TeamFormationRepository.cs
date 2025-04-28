using CapstoneShowcase.Core.Entities;
using CapstoneShowcase.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneShowcase.Infrastructure.Data.Repositories
{
    public class TeamFormationRepository : ITeamFormationRepository
    {
        private readonly AppDbContext _context;

        public TeamFormationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TeamFormationProfile> GetProfileByStudentIdAsync(string studentId)
        {
            return await _context.TeamFormationProfiles
                .Include(tfp => tfp.Student)
                    .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(p => p.StudentId == studentId);
        }

        public async Task<IReadOnlyList<TeamFormationProfile>> GetAllProfilesAsync()
        {
            return await _context.TeamFormationProfiles
                .Include(tfp => tfp.Student)
                    .ThenInclude(s => s.User)
                .Where(p => p.LookingForTeam)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<TeamFormationProfile>> GetProfilesByInterestsAsync(IEnumerable<string> interests)
        {
            var profiles = await _context.TeamFormationProfiles
                .Include(tfp => tfp.Student)
                    .ThenInclude(s => s.User)
                .Where(p => p.LookingForTeam)
                .ToListAsync();
                
            return profiles
                .Where(p => interests.Any(interest => 
                    p.Interests.Contains(interest)))
                .ToList();
        }

        public async Task<IReadOnlyList<TeamFormationProfile>> GetProfilesBySkillsAsync(IEnumerable<string> skills)
        {
            var profiles = await _context.TeamFormationProfiles
                .Include(tfp => tfp.Student)
                    .ThenInclude(s => s.User)
                .Where(p => p.LookingForTeam)
                .ToListAsync();
                
            return profiles
                .Where(p => skills.Any(skill => 
                    p.Skills.Contains(skill)))
                .ToList();
        }

        public async Task CreateOrUpdateProfileAsync(TeamFormationProfile profile)
        {
            var existingProfile = await _context.TeamFormationProfiles
                .FirstOrDefaultAsync(p => p.StudentId == profile.StudentId);
                
            if (existingProfile == null)
            {
                await _context.TeamFormationProfiles.AddAsync(profile);
            }
            else
            {
                existingProfile.InterestsJson = profile.InterestsJson;
                existingProfile.SkillsJson = profile.SkillsJson;
                existingProfile.LookingForTeam = profile.LookingForTeam;
                existingProfile.UpdatedAt = profile.UpdatedAt;
            }
            
            await _context.SaveChangesAsync();
        }
    }
}