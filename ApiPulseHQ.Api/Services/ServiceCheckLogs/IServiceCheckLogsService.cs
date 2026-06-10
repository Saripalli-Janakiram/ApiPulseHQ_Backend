using ApiPulseHQ.Api.Models.ServiceCheckLogs;

namespace ApiPulseHQ.Api.Services.ServiceCheckLogs
{
    public interface IServiceCheckLogsService
    {
        Task<List<ServiceCheckLogResponse>> GetLogsAsync(Guid endpointId, Guid userId);
        Task<ServiceCheckLogResponse?> GetLatestAsync(Guid endpointId, Guid userId);
        Task<UptimeSummaryResponse?> GetSummaryAsync(Guid endpointId, Guid userId);
    }
}
