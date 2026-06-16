using ApiPulseHQ.Api.Models.Auth;
using ApiPulseHQ.Application.DTOs.Auth;
using ApiPulseHQ.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiPulseHQ.Infrastructure.Persistence;        
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiPulseHQ.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApiPulseDbContext _db;
        public AuthController(IAuthService authService, ApiPulseDbContext db)
        {
            _authService = authService;
            _db = db;   
        }

        // -----------------------------
        // REGISTER
        // -----------------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var dto = new RegisterRequestDto
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password
            };

            var result = await _authService.RegisterAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // -----------------------------
        // LOGIN
        // -----------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var dto = new LoginRequestDto
            {
                Email = request.Email,
                Password = request.Password
            };

            var result = await _authService.LoginAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        // -----------------------------
        // REFRESH TOKEN
        // -----------------------------
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request.RefreshToken);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // -----------------------------
        // GET CURRENT USER (/auth/me)
        // -----------------------------
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = int.Parse(User.FindFirst("userId")!.Value);

            var user = await _db.Users.FindAsync(userId);

            return Ok(new
            {
                id = user.Id,
                username = user.Username,
                email = user.Email
            });
        }

        // -----------------------------
        // LOGOUT (/auth/logout)
        // -----------------------------
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = int.Parse(User.FindFirst("userId")!.Value);

            var user = await _db.Users.FindAsync(userId);

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Logged out successfully" });
        }


        // -----------------------------
        // FORGOT PASSWORD
        // -----------------------------
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest  request)
        {
            var dto = new ForgotPasswordRequestDto
            {
                Email = request.Email
            };

            await _authService.ForgotPasswordAsync(dto);

            return Ok(new { message = "If the email exists, a reset link has been sent." });
        }

        // -----------------------------
        // RESET PASSWORD
        // -----------------------------
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var dto = new ResetPasswordRequestDto
            {
                Token = request.Token,
                NewPassword = request.NewPassword
            };

            await _authService.ResetPasswordAsync(dto);

            return Ok(new { message = "Password reset successful" });
        }

        // -----------------------------
        // HELPER: Extract userId from JWT
        // -----------------------------
        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new Exception("Invalid token: userId missing");

            return Guid.Parse(userIdClaim);
        }
    }
}
