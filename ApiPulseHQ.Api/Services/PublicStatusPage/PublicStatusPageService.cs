using ApiPulseHQ.Api.Models.PublicStatusPage;
using ApiPulseHQ.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiPulseHQ.Api.Services.PublicStatusPage
{
    public class PublicStatusPageService : IPublicStatusPageService
    {
        private readonly ApiPulseDbContext _db;

        public PublicStatusPageService(ApiPulseDbContext db)
        {
            _db = db;
        }

        public async Task<PublicStatusPageResponse?> GetBySlugAsync(string slug)
        {
            var page = await _db.StatusPages
                .FirstOrDefaultAsync(x => x.Slug == slug && x.IsPublic);

            if (page == null)
                return null;

            var services = await _db.StatusPageServices
                .Where(x => x.StatusPageId == page.Id)
                .Select(x => new PublicStatusPageServiceItem
                {
                    ServiceEndpointId = x.ServiceEndpointId,
                    Name = x.ServiceEndpoint.Name,
                    Url = x.ServiceEndpoint.Url
                })
                .ToListAsync();

            return new PublicStatusPageResponse
            {
                Title = page.Title,
                Slug = page.Slug,
                Services = services
            };
        }
    }
}
