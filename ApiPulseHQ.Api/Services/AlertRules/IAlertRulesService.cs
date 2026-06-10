using ApiPulseHQ.Api.Models.AlertRules;

namespace ApiPulseHQ.Api.Services.AlertRules
{
    public interface IAlertRulesService
    {
        Task<List<AlertRuleResponse>> GetAllAsync(Guid userId);
        Task<AlertRuleResponse?> CreateAsync(Guid userId, CreateAlertRuleRequest request);
        Task<bool> DeleteAsync(Guid userId, Guid id);
    }
}
