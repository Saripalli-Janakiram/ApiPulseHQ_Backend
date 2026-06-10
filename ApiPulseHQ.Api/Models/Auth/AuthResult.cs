namespace ApiPulseHQ.Api.Models.Auth
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public TokenResponse? Tokens { get; set; }

        public static AuthResult SuccessResult(TokenResponse? tokens = null)
            => new AuthResult { Success = true, Tokens = tokens };

        public static AuthResult Fail(string message)
            => new AuthResult { Success = false, Message = message };
    }
}

