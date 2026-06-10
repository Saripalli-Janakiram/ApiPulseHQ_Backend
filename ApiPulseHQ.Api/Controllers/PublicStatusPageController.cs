using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPulseHQ.Infrastructure.Persistence;

namespace ApiPulseHQ.Api.Controllers
{
    [ApiController]
    [Route("public/status/{slug}")]
    public class PublicStatusPageController : ControllerBase
    {
        private readonly ApiPulseDbContext _db;

        public PublicStatusPageController(ApiPulseDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string slug)
        {
            var page = await _db.StatusPages
                .Include(x => x.StatusPageServices)              // FIXED
                .ThenInclude(s => s.ServiceEndpoint)             // FIXED
                .FirstOrDefaultAsync(x => x.Slug == slug && x.IsPublic);

            if (page == null)
                return NotFound();

            var response = new
            {
                page.Id,
                page.Title,                                      // FIXED (Name → Title)
                page.Description,
                Services = page.StatusPageServices.Select(s => new   // FIXED
                {
                    s.ServiceEndpoint.Name,
                    s.ServiceEndpoint.Url,
                    s.ServiceEndpoint.IsActive
                })
            };

            return Ok(response);
        }
    }
}
