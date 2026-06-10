namespace ApiPulseHQ.Api.Models.ServiceCheckLogs
{
    public class ServiceCheckLogResponse
    {
        public Guid Id { get; set; }
        public int StatusCode { get; set; }
        public long ResponseTimeMs { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime CheckedAt { get; set; }
    }
}
