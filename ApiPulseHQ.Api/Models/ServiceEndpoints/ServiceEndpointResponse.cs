namespace ApiPulseHQ.Api.Models.ServiceEndpoints
{
    public class ServiceEndpointResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int CheckIntervalSeconds { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
