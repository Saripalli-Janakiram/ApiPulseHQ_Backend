namespace ApiPulseHQ.Api.Models.AlertRules
{
    public class AlertRuleResponse
    {
        public Guid Id { get; set; }
        public Guid ServiceEndpointId { get; set; }
        public bool AlertOnFailure { get; set; }
        public bool AlertOnRecovery { get; set; }
        public string Email { get; set; } = string.Empty;
        public int CooldownMinutes { get; set; }
        public DateTime? LastAlertSentAt { get; set; }
    }
}
