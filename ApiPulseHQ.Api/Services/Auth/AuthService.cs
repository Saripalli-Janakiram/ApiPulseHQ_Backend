using ApiPulseHQ.Api.Models.Auth;
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

        public async Task<AuthResult> RegisterAsync(RegisterRequest request)
        {
            var email = request.Email.Trim().ToLower();

            if (await _db.Users.AnyAsync(u => u.Email == email))
                return AuthResult.Fail("Email already registered");

            var user = new User
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return AuthResult.SuccessResult();
        }

        public async Task<AuthResult> LoginAsync(LoginRequest request)
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
