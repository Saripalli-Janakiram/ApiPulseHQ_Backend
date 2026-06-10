namespace ApiPulseHQ.Domain.Entities
{
    public class StatusPageService
    {
        public Guid Id { get; set; }

        public Guid StatusPageId { get; set; }
        public Guid ServiceEndpointId { get; set; }

        // REQUIRED — this is what your service layer expects
        public bool IsActive { get; set; } = true;

        public StatusPage StatusPage { get; set; } = default!;
        public ServiceEndpoint ServiceEndpoint { get; set; } = default!;
    }
}
