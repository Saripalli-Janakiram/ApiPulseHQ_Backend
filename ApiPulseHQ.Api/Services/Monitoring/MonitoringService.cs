using ApiPulseHQ.Infrastructure.Persistence;
using ApiPulseHQ.Domain.Entities;
using ApiPulseHQ.Api.Services.Email;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ApiPulseHQ.Api.Services.Monitoring
{
    public class MonitoringService : IMonitoringService
    {
        private readonly ApiPulseDbContext _db;
        private readonly HttpClient _http;
        private readonly IEmailSender _email;

        public MonitoringService(ApiPulseDbContext db, IHttpClientFactory httpClientFactory, IEmailSender email)
        {
            _db = db;
            _http = httpClientFactory.CreateClient();
            _email = email;
        }

        public async Task CheckAllAsync()
        {
            var endpoints = await _db.ServiceEndpoints
                .Where(x => x.IsActive)
                .ToListAsync();

            foreach (var endpoint in endpoints)
            {
                await CheckEndpointAsync(endpoint.Id, endpoint.Url);
            }
        }

        private async Task CheckEndpointAsync(Guid endpointId, string url)
        {
            var stopwatch = Stopwatch.StartNew();
            int statusCode = 0;
            bool success = false;

            try
            {
                var response = await _http.GetAsync(url);
                statusCode = (int)response.StatusCode;
                success = response.IsSuccessStatusCode;
            }
            catch
            {
                statusCode = 0;
                success = false;
            }

            stopwatch.Stop();

            var log = new ServiceCheckLog
            {
                Id = Guid.NewGuid(),
                ServiceEndpointId = endpointId,
                CheckedAt = DateTime.UtcNow,
                StatusCode = statusCode,
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                IsSuccess = success
            };

            _db.ServiceCheckLogs.Add(log);
            await _db.SaveChangesAsync();

            // ⭐ ADD THIS — Trigger alerts after logging
            await TriggerAlertsAsync(endpointId, success);
        }

        // ⭐ NEW METHOD — Alert Logic
        private async Task TriggerAlertsAsync(Guid endpointId, bool isSuccess)
        {
            var rules = await _db.AlertRules
                .Where(x => x.ServiceEndpointId == endpointId && x.IsActive)
                .ToListAsync();

            foreach (var rule in rules)
            {
                bool shouldSend = false;

                // Failure alert
                if (!isSuccess && rule.AlertOnFailure)
                    shouldSend = true;

                // Recovery alert
                if (isSuccess && rule.AlertOnRecovery)
                    shouldSend = true;

                // Cooldown check
                if (rule.LastAlertSentAt != null &&
                    rule.LastAlertSentAt > DateTime.UtcNow.AddMinutes(-rule.CooldownMinutes))
                    continue;

                if (shouldSend)
                {
                    string subject = isSuccess
                        ? "Service Recovered"
                        : "Service Down";

                    string body = isSuccess
                        ? "Your service endpoint is now back online."
                        : "Your service endpoint is currently unreachable.";

                    await _email.SendAsync(rule.NotificationEmail, subject, body);

                    rule.LastAlertSentAt = DateTime.UtcNow;
                }
            }

            await _db.SaveChangesAsync();
        }
    }
}
