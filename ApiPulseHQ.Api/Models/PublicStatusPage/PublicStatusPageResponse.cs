namespace ApiPulseHQ.Api.Models.PublicStatusPage
{
    public class PublicStatusPageResponse
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public List<PublicStatusPageServiceItem> Services { get; set; } = new();
    }

    public class PublicStatusPageServiceItem
    {
        public Guid ServiceEndpointId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        // Later we will add:
        // public string CurrentStatus { get; set; }
        // public DateTime LastCheckedAt { get; set; }
    }
}
