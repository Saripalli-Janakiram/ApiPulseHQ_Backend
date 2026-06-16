using ApiPulseHQ.Application.Interfaces;
using ApiPulseHQ.Application.DTOs.Auth;
using ApiPulseHQ.Domain.Entities;
using ApiPulseHQ.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiPulseHQ.Api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ApiPulseDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(ApiPulseDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        // -----------------------------
        // REGISTER
        // -----------------------------
        public async Task<AuthResult> RegisterAsync(RegisterRequestDto request)
        {
            var email = request.Email.Trim().ToLower();
            var username = request.Username.Trim().ToLower();

            if (await _db.Users.AnyAsync(u => u.Email == email))
                return AuthResult.Fail("Email already registered");

            if (await _db.Users.AnyAsync(u => u.Username == username))
                return AuthResult.Fail("Username already taken");

            var user = new User
            {
                Username = request.Username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return AuthResult.SuccessResult();
        }


        // -----------------------------
        // LOGIN
        // -----------------------------
        public async Task<AuthResult> LoginAsync(LoginRequestDto request)
        {
            var email = request.Email.Trim().ToLower();

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return AuthResult.Fail("Invalid email or password");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return AuthResult.Fail("Invalid email or password");

            var tokens = GenerateTokens(user);

            user.RefreshToken = tokens.RefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _db.SaveChangesAsync();

            return AuthResult.SuccessResult(tokens);
        }

        // -----------------------------
        // REFRESH TOKEN
        // -----------------------------
        public async Task<AuthResult> RefreshTokenAsync(string refreshToken)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return AuthResult.Fail("Invalid or expired refresh token");

            var tokens = GenerateTokens(user);

            user.RefreshToken = tokens.RefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _db.SaveChangesAsync();

            return AuthResult.SuccessResult(tokens);
        }

        // -----------------------------
        // GET CURRENT USER (/auth/me)
        // -----------------------------
        public async Task<UserProfileResponse> GetCurrentUserAsync(Guid userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new Exception("User not found");

            return new UserProfileResponse
            {
                Id = user.Id,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            };
        }

        // -----------------------------
        // LOGOUT (/auth/logout)
        // -----------------------------
        public async Task LogoutAsync(Guid userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return;

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            await _db.SaveChangesAsync();
        }

        // -----------------------------
        // FORGOT PASSWORD
        // -----------------------------
        public async Task ForgotPasswordAsync(ForgotPasswordRequestDto request)
        {
            var email = request.Email.Trim().ToLower();

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return; // Do not reveal user existence

            user.PasswordResetToken = Guid.NewGuid().ToString("N");
            user.PasswordResetExpiry = DateTime.UtcNow.AddHours(1);

            await _db.SaveChangesAsync();

            await SendPasswordResetEmailStub(user.Email, user.PasswordResetToken);
        }

        // -----------------------------
        // RESET PASSWORD
        // -----------------------------
        public async Task ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u =>
                u.PasswordResetToken == request.Token &&
                u.PasswordResetExpiry > DateTime.UtcNow);

            if (user == null)
                throw new Exception("Invalid or expired password reset token");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetExpiry = null;

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            await _db.SaveChangesAsync();
        }

        // -----------------------------
        // EMAIL STUB
        // -----------------------------
        private Task SendPasswordResetEmailStub(string email, string token)
        {
            Console.WriteLine($"[EMAIL STUB] Password reset token for {email}: {token}");
            return Task.CompletedTask;
        }

        // -----------------------------
        // GENERATE TOKENS
        // -----------------------------
        private TokenResponse GenerateTokens(User user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var keyString = jwtSection["Key"];

            if (string.IsNullOrWhiteSpace(keyString) || keyString.Length < 32)
                throw new Exception("JWT Key must be at least 32 characters long.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds,
                claims: new[]
                {
                    new Claim("userId", user.Id.ToString())
                }
            );

            return new TokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = Guid.NewGuid().ToString()
            };
        }
    }
}
