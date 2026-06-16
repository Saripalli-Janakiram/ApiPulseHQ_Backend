using System;

namespace ApiPulseHQ.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = default!;

        public string PasswordHash { get; set; } = default!;

        // Refresh Token (already present)
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        // Password Reset Token (NEW)
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetExpiry { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
