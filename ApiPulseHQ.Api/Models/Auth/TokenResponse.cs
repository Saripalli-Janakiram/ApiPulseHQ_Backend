namespace ApiPulseHQ.Api.Models.Auth
{
    public class TokenResponse
    {
        public string Token { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}
