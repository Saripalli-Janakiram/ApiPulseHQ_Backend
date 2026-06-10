using ApiPulseHQ.Api.Models.StatusPages;
using ApiPulseHQ.Domain.Entities;
using ApiPulseHQ.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiPulseHQ.Api.Services.StatusPages
{
    public class StatusPageServicesService : IStatusPageServicesService
    {
        private readonly ApiPulseDbContext _db;

        public StatusPageServicesService(ApiPulseDbContext db)
        {
            _db = db;
        }

        public async Task<List<StatusPageServiceResponse>> GetAllAsync(Guid pageId, Guid userId)
        {
            return await _db.StatusPageServices
                .Where(x => x.StatusPageId == pageId && x.StatusPage.UserId == userId)
                .Select(x => new StatusPageServiceResponse
                {
                    Id = x.Id,
                    ServiceEndpointId = x.ServiceEndpointId,
                    Name = x.ServiceEndpoint.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();
        }

        public async Task<StatusPageServiceResponse?> AddAsync(Guid pageId, Guid userId, StatusPageServiceRequest request)
        {
            var page = await _db.StatusPages
                .FirstOrDefaultAsync(x => x.Id == pageId && x.UserId == userId);

            if (page == null)
                return null;

            var service = new StatusPageService
            {
                Id = Guid.NewGuid(),
                StatusPageId = pageId,
                ServiceEndpointId = request.ServiceEndpointId,
                IsActive = true
            };

            _db.StatusPageServices.Add(service);
            await _db.SaveChangesAsync();

            return new StatusPageServiceResponse
            {
                Id = service.Id,
                ServiceEndpointId = service.ServiceEndpointId,
                Name = (await _db.ServiceEndpoints.FindAsync(service.ServiceEndpointId))!.Name,
                IsActive = service.IsActive
            };
        }

        public async Task<bool> RemoveAsync(Guid pageId, Guid userId, Guid id)
        {
            var service = await _db.StatusPageServices
                .Include(x => x.StatusPage)
                .FirstOrDefaultAsync(x => x.Id == id && x.StatusPageId == pageId && x.StatusPage.UserId == userId);

            if (service == null)
                return false;

            _db.StatusPageServices.Remove(service);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
