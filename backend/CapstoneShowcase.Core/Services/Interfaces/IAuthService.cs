using CapstoneShowcase.Core.DTOs.Auth;
using System.Threading.Tasks;

namespace CapstoneShowcase.Core.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDto> RegisterAsync(RegisterDto registerDto, string role);
        Task<TokenDto> LoginAsync(LoginDto loginDto);
        Task<TokenDto> RefreshTokenAsync(string refreshToken);
        Task<bool> ValidateTokenAsync(string token);
    }
}