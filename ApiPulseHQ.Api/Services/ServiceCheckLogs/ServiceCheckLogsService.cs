using ApiPulseHQ.Api.Models.ServiceCheckLogs;
using ApiPulseHQ.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiPulseHQ.Api.Services.ServiceCheckLogs
{
    public class ServiceCheckLogsService : IServiceCheckLogsService
    {
        private readonly ApiPulseDbContext _db;

        public ServiceCheckLogsService(ApiPulseDbContext db)
        {
            _db = db;
        }

        public async Task<List<ServiceCheckLogResponse>> GetLogsAsync(Guid endpointId, Guid userId)
        {
            var endpoint = await _db.ServiceEndpoints
                .FirstOrDefaultAsync(x => x.Id == endpointId && x.UserId == userId);

            if (endpoint == null)
                return new List<ServiceCheckLogResponse>();

            return await _db.ServiceCheckLogs
                .Where(x => x.ServiceEndpointId == endpointId)
                .OrderByDescending(x => x.CheckedAt)
                .Select(x => new ServiceCheckLogResponse
                {
                    Id = x.Id,
                    StatusCode = x.StatusCode,
                    ResponseTimeMs = x.ResponseTimeMs,
                    IsSuccess = x.IsSuccess,
                    CheckedAt = x.CheckedAt
                })
                .ToListAsync();
        }

        public async Task<ServiceCheckLogResponse?> GetLatestAsync(Guid endpointId, Guid userId)
        {
            var endpoint = await _db.ServiceEndpoints
                .FirstOrDefaultAsync(x => x.Id == endpointId && x.UserId == userId);

            if (endpoint == null)
                return null;

            return await _db.ServiceCheckLogs
                .Where(x => x.ServiceEndpointId == endpointId)
                .OrderByDescending(x => x.CheckedAt)
                .Select(x => new ServiceCheckLogResponse
                {
                    Id = x.Id,
                    StatusCode = x.StatusCode,
                    ResponseTimeMs = x.ResponseTimeMs,
                    IsSuccess = x.IsSuccess,
                    CheckedAt = x.CheckedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<UptimeSummaryResponse?> GetSummaryAsync(Guid endpointId, Guid userId)
        {
            var endpoint = await _db.ServiceEndpoints
                .FirstOrDefaultAsync(x => x.Id == endpointId && x.UserId == userId);

            if (endpoint == null)
                return null;

            var logs = await _db.ServiceCheckLogs
                .Where(x => x.ServiceEndpointId == endpointId)
                .ToListAsync();

            if (!logs.Any())
                return new UptimeSummaryResponse
                {
                    TotalChecks = 0,
                    SuccessCount = 0,
                    FailureCount = 0,
                    UptimePercentage = 0,
                    LastCheckedAt = null
                };

            int total = logs.Count;
            int success = logs.Count(x => x.IsSuccess);
            int failure = total - success;

            return new UptimeSummaryResponse
            {
                TotalChecks = total,
                SuccessCount = success,
                FailureCount = failure,
                UptimePercentage = Math.Round((double)success / total * 100, 2),
                LastCheckedAt = logs.Max(x => x.CheckedAt)
            };
        }
    }
}
