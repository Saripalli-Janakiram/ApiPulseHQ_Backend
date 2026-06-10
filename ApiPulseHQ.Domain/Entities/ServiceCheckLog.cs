namespace ApiPulseHQ.Domain.Entities
{
    public class ServiceCheckLog
    {
        public Guid Id { get; set; }
        public Guid ServiceEndpointId { get; set; }
        public int StatusCode { get; set; }
        public long ResponseTimeMs { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime CheckedAt { get; set; }

        public ServiceEndpoint ServiceEndpoint { get; set; } = default!;
    }
}
