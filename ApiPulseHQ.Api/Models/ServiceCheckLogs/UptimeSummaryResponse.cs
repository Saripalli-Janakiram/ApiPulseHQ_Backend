namespace ApiPulseHQ.Api.Models.ServiceCheckLogs
{
    public class UptimeSummaryResponse
    {
        public int TotalChecks { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public double UptimePercentage { get; set; }
        public DateTime? LastCheckedAt { get; set; }
    }
}
