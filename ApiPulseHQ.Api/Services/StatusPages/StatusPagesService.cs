using ApiPulseHQ.Api.Models.StatusPages;
using ApiPulseHQ.Infrastructure.Persistence;
using ApiPulseHQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiPulseHQ.Api.Services.StatusPages
{
    public class StatusPagesService : IStatusPagesService
    {
        private readonly ApiPulseDbContext _db;

        public StatusPagesService(ApiPulseDbContext db)
        {
            _db = db;
        }

        public async Task<List<StatusPageResponse>> GetAllAsync(Guid userId)
        {
            return await _db.StatusPages
                .Where(x => x.UserId == userId)
                .Select(x => new StatusPageResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug = x.Slug,
                    IsPublic = x.IsPublic,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<StatusPageResponse?> GetByIdAsync(Guid userId, Guid id)
        {
            return await _db.StatusPages
                .Where(x => x.UserId == userId && x.Id == id)
                .Select(x => new StatusPageResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug = x.Slug,
                    IsPublic = x.IsPublic,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<StatusPageResponse> CreateAsync(Guid userId, CreateStatusPageRequest request)
        {
            var entity = new StatusPage
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = request.Title,
                Slug = request.Slug,
                IsPublic = request.IsPublic,
                CreatedAt = DateTime.UtcNow
            };

            _db.StatusPages.Add(entity);
            await _db.SaveChangesAsync();

            return new StatusPageResponse
            {
                Id = entity.Id,
                Title = entity.Title,
                Slug = entity.Slug,
                IsPublic = entity.IsPublic,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<StatusPageResponse?> UpdateAsync(Guid userId, Guid id, UpdateStatusPageRequest request)
        {
            var entity = await _db.StatusPages
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);

            if (entity == null)
                return null;

            entity.Title = request.Title;
            entity.Slug = request.Slug;
            entity.IsPublic = request.IsPublic;

            await _db.SaveChangesAsync();

            return new StatusPageResponse
            {
                Id = entity.Id,
                Title = entity.Title,
                Slug = entity.Slug,
                IsPublic = entity.IsPublic,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid id)
        {
            var entity = await _db.StatusPages
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);

            if (entity == null)
                return false;

            _db.StatusPages.Remove(entity);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
