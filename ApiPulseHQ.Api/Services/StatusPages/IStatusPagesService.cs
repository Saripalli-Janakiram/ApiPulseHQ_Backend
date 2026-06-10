using ApiPulseHQ.Api.Models.StatusPages;

namespace ApiPulseHQ.Api.Services.StatusPages
{
    public interface IStatusPagesService
    {
        Task<List<StatusPageResponse>> GetAllAsync(Guid userId);
        Task<StatusPageResponse?> GetByIdAsync(Guid userId, Guid id);
        Task<StatusPageResponse> CreateAsync(Guid userId, CreateStatusPageRequest request);
        Task<StatusPageResponse?> UpdateAsync(Guid userId, Guid id, UpdateStatusPageRequest request);
        Task<bool> DeleteAsync(Guid userId, Guid id);
    }
}
