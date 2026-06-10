namespace ApiPulseHQ.Api.Models.StatusPages
{
    public class StatusPageResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
