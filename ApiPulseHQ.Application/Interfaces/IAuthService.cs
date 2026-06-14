using ApiPulseHQ.Application.DTOs.Auth;

namespace ApiPulseHQ.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterRequestDto request);
        Task<AuthResult> LoginAsync(LoginRequestDto request);
        Task<AuthResult> RefreshTokenAsync(string refreshToken);

        Task<UserProfileResponse> GetCurrentUserAsync(Guid userId);
        Task LogoutAsync(Guid userId);
        Task ForgotPasswordAsync(ForgotPasswordRequestDto request);
        Task ResetPasswordAsync(ResetPasswordRequestDto request);
    }
}
