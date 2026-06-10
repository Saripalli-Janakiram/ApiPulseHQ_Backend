namespace ApiPulseHQ.Api.Models.StatusPages
{
    public class UpdateStatusPageRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
    }
}
