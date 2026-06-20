namespace ApiPulseHQ.Application.DTOs.Auth
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }

        // ⭐ Add these two properties
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }

        public static AuthResult SuccessResult(string token, string refreshToken)
        {
            return new AuthResult
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken
            };
        }

        public static AuthResult Fail(string error)
        {
            return new AuthResult
            {
                Success = false,
                Error = error
            };
        }
    }
}
