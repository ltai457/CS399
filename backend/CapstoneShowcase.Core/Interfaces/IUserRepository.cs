using System.Collections.Generic;
using System.Threading.Tasks;
using CapstoneShowcase.Core.Entities;

namespace CapstoneShowcase.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<Student> GetStudentByIdAsync(string userId);
        Task<IReadOnlyList<Student>> GetAllStudentsAsync();
        Task<IReadOnlyList<Student>> GetStudentsLookingForTeamAsync();
        Task<bool> IsUserInRoleAsync(string userId, string role);
    }
}