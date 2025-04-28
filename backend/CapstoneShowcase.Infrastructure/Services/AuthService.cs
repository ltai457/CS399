using CapstoneShowcase.Core.DTOs.Auth;
using CapstoneShowcase.Core.Entities;
using CapstoneShowcase.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneShowcase.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _configuration;
        
        public AuthService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            TokenService tokenService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _configuration = configuration;
        }
        
        public async Task<TokenDto> RegisterAsync(RegisterDto registerDto, string role)
        {
            // Check if user exists
            var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userExists != null)
                throw new Exception($"User with email {registerDto.Email} already exists");
                
            // Check if role exists
            if (!await _roleManager.RoleExistsAsync(role))
                throw new Exception($"Role {role} does not exist");
                
            // Create user
            var user = new User
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            
            if (!result.Succeeded)
                throw new Exception($"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                
            // Add user to role
            await _userManager.AddToRoleAsync(user, role);
            
            // Create student profile if role is Student
            if (role == "Student" && registerDto.Skills != null && registerDto.Interests != null)
            {
                var student = new Student
                {
                    UserId = user.Id,
                    Skills = registerDto.Skills.ToList(),
                    Interests = registerDto.Interests.ToList(),
                    GithubProfile = registerDto.GithubProfile,
                    LinkedInProfile = registerDto.LinkedInProfile,
                    Biography = registerDto.Biography
                };
                
                // Save student profile (this would be handled by your repository)
                // _studentRepository.AddAsync(student);
            }
            
            // Generate token
            return await GenerateTokenDto(user);
        }
        
        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                throw new Exception("Invalid email or password");
                
            return await GenerateTokenDto(user);
        }
        
        public async Task<TokenDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users.Include(u => u.RefreshToken)
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new Exception("Invalid or expired refresh token");
            return await GenerateTokenDto(user);
        }
        
        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(token);
                var userId = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                
                if (string.IsNullOrEmpty(userId))
                    return false;
                    
                var user = await _userManager.FindByIdAsync(userId);
                return user != null;
            }
            catch
            {
                return false;
            }
        }
        
        private async Task<TokenDto> GenerateTokenDto(User user)
        {
            var jwtToken = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            
            // Update refresh token in database
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); // Token valid for 7 days
            await _userManager.UpdateAsync(user);
            
            // Get roles
            var roles = await _userManager.GetRolesAsync(user);
            
            return new TokenDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = refreshToken,
                ExpiresIn = (long)(jwtToken.ValidTo - DateTime.Now).TotalSeconds,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfilePicture = user.ProfilePicture,
                    Roles = roles.ToArray()
                }
            };
        }
    }
}