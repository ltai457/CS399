using CapstoneShowcase.Core.Entities;
using CapstoneShowcase.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneShowcase.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Student> GetStudentByIdAsync(string userId)
        {
            return await _context.Students
                .Include(s => s.User)
                .SingleOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<IReadOnlyList<Student>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Include(s => s.User)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Student>> GetStudentsLookingForTeamAsync()
        {
            return await _context.Students
                .Include(s => s.User)
                .Include(s => s.TeamFormationProfile)
                .Where(s => s.TeamFormationProfile != null && s.TeamFormationProfile.LookingForTeam)
                .ToListAsync();
        }

        public async Task<bool> IsUserInRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;
                
            return await _userManager.IsInRoleAsync(user, role);
        }
    }
}