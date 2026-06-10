namespace ApiPulseHQ.Api.Models.ServiceEndpoints
{
    public class UpdateServiceEndpointRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int CheckIntervalSeconds { get; set; }
        public bool IsActive { get; set; }
    }
}
