using ApiPulseHQ.Api.Models.ServiceEndpoints;

namespace ApiPulseHQ.Api.Services.ServiceEndpoints
{
    public interface IServiceEndpointsService
    {
        Task<List<ServiceEndpointResponse>> GetAllAsync(Guid userId);
        Task<ServiceEndpointResponse?> GetByIdAsync(Guid userId, Guid id);
        Task<ServiceEndpointResponse> CreateAsync(Guid userId, CreateServiceEndpointRequest request);
        Task<ServiceEndpointResponse?> UpdateAsync(Guid userId, Guid id, UpdateServiceEndpointRequest request);
        Task<bool> DeleteAsync(Guid userId, Guid id);
    }
}
