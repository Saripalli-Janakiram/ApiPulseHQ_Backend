using ApiPulseHQ.Api.Models.ServiceEndpoints;
using ApiPulseHQ.Infrastructure.Persistence;
using ApiPulseHQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiPulseHQ.Api.Services.ServiceEndpoints
{
    public class ServiceEndpointsService : IServiceEndpointsService
    {
        private readonly ApiPulseDbContext _db;

        public ServiceEndpointsService(ApiPulseDbContext db)
        {
            _db = db;
        }

        public async Task<List<ServiceEndpointResponse>> GetAllAsync(Guid userId)
        {
            return await _db.ServiceEndpoints
                .Where(x => x.UserId == userId)
                .Select(x => new ServiceEndpointResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Url = x.Url,
                    CheckIntervalSeconds = x.CheckIntervalSeconds,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ServiceEndpointResponse?> GetByIdAsync(Guid userId, Guid id)
        {
            return await _db.ServiceEndpoints
                .Where(x => x.UserId == userId && x.Id == id)
                .Select(x => new ServiceEndpointResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Url = x.Url,
                    CheckIntervalSeconds = x.CheckIntervalSeconds,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ServiceEndpointResponse> CreateAsync(Guid userId, CreateServiceEndpointRequest request)
        {
            var entity = new ServiceEndpoint
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = request.Name,
                Url = request.Url,
                CheckIntervalSeconds = request.CheckIntervalSeconds,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.ServiceEndpoints.Add(entity);
            await _db.SaveChangesAsync();

            return new ServiceEndpointResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Url = entity.Url,
                CheckIntervalSeconds = entity.CheckIntervalSeconds,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<ServiceEndpointResponse?> UpdateAsync(Guid userId, Guid id, UpdateServiceEndpointRequest request)
        {
            var entity = await _db.ServiceEndpoints
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);

            if (entity == null)
                return null;

            entity.Name = request.Name;
            entity.Url = request.Url;
            entity.CheckIntervalSeconds = request.CheckIntervalSeconds;
            entity.IsActive = request.IsActive;

            await _db.SaveChangesAsync();

            return new ServiceEndpointResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Url = entity.Url,
                CheckIntervalSeconds = entity.CheckIntervalSeconds,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid id)
        {
            var entity = await _db.ServiceEndpoints
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);

            if (entity == null)
                return false;

            _db.ServiceEndpoints.Remove(entity);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
