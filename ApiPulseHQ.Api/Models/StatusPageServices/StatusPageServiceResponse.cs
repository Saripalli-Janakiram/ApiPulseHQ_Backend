namespace ApiPulseHQ.Api.Models.StatusPageServices
{
    public class StatusPageServiceResponse
    {
        public Guid Id { get; set; }
        public Guid StatusPageId { get; set; }
        public Guid ServiceEndpointId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceUrl { get; set; } = string.Empty;
    }
}
