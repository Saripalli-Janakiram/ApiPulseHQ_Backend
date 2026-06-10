using ApiPulseHQ.Api.Models.StatusPageServices;

namespace ApiPulseHQ.Api.Services.StatusPageServices
{
    public interface IStatusPageServicesService
    {
        Task<List<StatusPageServiceResponse>> GetServicesAsync(Guid userId, Guid statusPageId);
        Task<StatusPageServiceResponse?> AddServiceAsync(Guid userId, Guid statusPageId, Guid serviceEndpointId);
        Task<bool> RemoveServiceAsync(Guid userId, Guid statusPageServiceId);
    }
}
