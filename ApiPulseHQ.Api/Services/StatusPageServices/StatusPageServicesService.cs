using ApiPulseHQ.Api.Models.StatusPageServices;
using ApiPulseHQ.Infrastructure.Persistence;
using ApiPulseHQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiPulseHQ.Api.Services.StatusPageServices
{
    public class StatusPageServicesService : IStatusPageServicesService
    {
        private readonly ApiPulseDbContext _db;

        public StatusPageServicesService(ApiPulseDbContext db)
        {
            _db = db;
        }

        public async Task<List<StatusPageServiceResponse>> GetServicesAsync(Guid userId, Guid statusPageId)
        {
            return await _db.StatusPageServices
                .Where(x => x.StatusPage.UserId == userId && x.StatusPageId == statusPageId)
                .Select(x => new StatusPageServiceResponse
                {
                    Id = x.Id,
                    StatusPageId = x.StatusPageId,
                    ServiceEndpointId = x.ServiceEndpointId,
                    ServiceName = x.ServiceEndpoint.Name,
                    ServiceUrl = x.ServiceEndpoint.Url
                })
                .ToListAsync();
        }

        public async Task<StatusPageServiceResponse?> AddServiceAsync(Guid userId, Guid statusPageId, Guid serviceEndpointId)
        {
            var statusPage = await _db.StatusPages
                .FirstOrDefaultAsync(x => x.Id == statusPageId && x.UserId == userId);

            if (statusPage == null)
                return null;

            var serviceEndpoint = await _db.ServiceEndpoints
                .FirstOrDefaultAsync(x => x.Id == serviceEndpointId && x.UserId == userId);

            if (serviceEndpoint == null)
                return null;

            var entity = new StatusPageService
            {
                Id = Guid.NewGuid(),
                StatusPageId = statusPageId,
                ServiceEndpointId = serviceEndpointId
            };

            _db.StatusPageServices.Add(entity);
            await _db.SaveChangesAsync();

            return new StatusPageServiceResponse
            {
                Id = entity.Id,
                StatusPageId = entity.StatusPageId,
                ServiceEndpointId = entity.ServiceEndpointId,
                ServiceName = serviceEndpoint.Name,
                ServiceUrl = serviceEndpoint.Url
            };
        }

        public async Task<bool> RemoveServiceAsync(Guid userId, Guid statusPageServiceId)
        {
            var entity = await _db.StatusPageServices
                .Include(x => x.StatusPage)
                .FirstOrDefaultAsync(x => x.Id == statusPageServiceId && x.StatusPage.UserId == userId);

            if (entity == null)
                return false;

            _db.StatusPageServices.Remove(entity);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
