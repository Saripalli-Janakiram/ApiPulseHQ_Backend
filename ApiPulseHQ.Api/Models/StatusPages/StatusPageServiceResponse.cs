namespace ApiPulseHQ.Api.Models.StatusPages
{
    public class StatusPageServiceResponse
    {
        public Guid Id { get; set; }
        public Guid ServiceEndpointId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
