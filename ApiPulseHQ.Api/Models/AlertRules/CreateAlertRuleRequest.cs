namespace ApiPulseHQ.Api.Models.AlertRules
{
    public class CreateAlertRuleRequest
    {
        public Guid ServiceEndpointId { get; set; }
        public bool AlertOnFailure { get; set; } = true;
        public bool AlertOnRecovery { get; set; } = true;
        public string Email { get; set; } = string.Empty;
        public int CooldownMinutes { get; set; } = 30;
    }
}
