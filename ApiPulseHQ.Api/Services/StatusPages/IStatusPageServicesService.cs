using ApiPulseHQ.Api.Models.StatusPages;

namespace ApiPulseHQ.Api.Services.StatusPages
{
    public interface IStatusPageServicesService
    {
        Task<List<StatusPageServiceResponse>> GetAllAsync(Guid pageId, Guid userId);
        Task<StatusPageServiceResponse?> AddAsync(Guid pageId, Guid userId, StatusPageServiceRequest request);
        Task<bool> RemoveAsync(Guid pageId, Guid userId, Guid id);
    }
}
