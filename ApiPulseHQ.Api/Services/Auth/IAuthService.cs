using ApiPulseHQ.Api.Models.Auth;

using System.Threading.Tasks;

namespace ApiPulseHQ.Api.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterRequest request);
        Task<AuthResult> LoginAsync(LoginRequest request);
        Task<AuthResult> RefreshTokenAsync(string refreshToken);
    }
}

