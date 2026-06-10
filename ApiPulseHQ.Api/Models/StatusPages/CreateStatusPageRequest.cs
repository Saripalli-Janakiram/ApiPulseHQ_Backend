namespace ApiPulseHQ.Api.Models.StatusPages
{
    public class CreateStatusPageRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
    }
}
