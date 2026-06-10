using ApiPulseHQ.Api.Models.AlertRules;
using ApiPulseHQ.Infrastructure.Persistence;
using ApiPulseHQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiPulseHQ.Api.Services.AlertRules
{
    public class AlertRulesService : IAlertRulesService
    {
        private readonly ApiPulseDbContext _db;

        public AlertRulesService(ApiPulseDbContext db)
        {
            _db = db;
        }

        public async Task<List<AlertRuleResponse>> GetAllAsync(Guid userId)
        {
            return await _db.AlertRules
                .Where(x => x.UserId == userId)
                .Select(x => new AlertRuleResponse
                {
                    Id = x.Id,
                    ServiceEndpointId = x.ServiceEndpointId,
                    AlertOnFailure = x.AlertOnFailure,
                    AlertOnRecovery = x.AlertOnRecovery,
                    Email = x.NotificationEmail,          // FIXED
                    CooldownMinutes = x.CooldownMinutes,
                    LastAlertSentAt = x.LastAlertSentAt
                })
                .ToListAsync();
        }

        public async Task<AlertRuleResponse?> CreateAsync(Guid userId, CreateAlertRuleRequest request)
        {
            var endpoint = await _db.ServiceEndpoints
                .FirstOrDefaultAsync(x => x.Id == request.ServiceEndpointId && x.UserId == userId);

            if (endpoint == null)
                return null;

            var rule = new AlertRule
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ServiceEndpointId = request.ServiceEndpointId,
                AlertOnFailure = request.AlertOnFailure,
                AlertOnRecovery = request.AlertOnRecovery,
                NotificationEmail = request.Email,      // FIXED
                CooldownMinutes = request.CooldownMinutes,
                IsActive = true                         // optional but recommended
            };

            _db.AlertRules.Add(rule);
            await _db.SaveChangesAsync();

            return new AlertRuleResponse
            {
                Id = rule.Id,
                ServiceEndpointId = rule.ServiceEndpointId,
                AlertOnFailure = rule.AlertOnFailure,
                AlertOnRecovery = rule.AlertOnRecovery,
                Email = rule.NotificationEmail,         // FIXED
                CooldownMinutes = rule.CooldownMinutes,
                LastAlertSentAt = rule.LastAlertSentAt
            };
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid id)
        {
            var rule = await _db.AlertRules
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (rule == null)
                return false;

            _db.AlertRules.Remove(rule);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
