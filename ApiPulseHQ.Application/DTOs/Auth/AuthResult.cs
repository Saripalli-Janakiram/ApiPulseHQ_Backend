namespace ApiPulseHQ.Application.DTOs.Auth
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public TokenResponse? Tokens { get; set; }

        public static AuthResult SuccessResult(TokenResponse? tokens = null)
        {
            return new AuthResult
            {
                Success = true,
                Tokens = tokens
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
