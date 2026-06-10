namespace ApiPulseHQ.Domain.Entities
{
    public class AlertRule
    {
        public Guid Id { get; set; }

        // Required for multi-tenant security
        public Guid UserId { get; set; }

        public Guid ServiceEndpointId { get; set; }

        // Your existing fields (unchanged)
        public string Condition { get; set; } = default!; // e.g. "DOWN", "SLOW"
        public int ThresholdSeconds { get; set; }
        public string NotificationEmail { get; set; } = default!;
        public bool IsActive { get; set; }

        // New fields required by alerting system
        public bool AlertOnFailure { get; set; } = true;
        public bool AlertOnRecovery { get; set; } = true;

        public int CooldownMinutes { get; set; } = 30;
        public DateTime? LastAlertSentAt { get; set; }

        public ServiceEndpoint ServiceEndpoint { get; set; } = default!;
    }
}
