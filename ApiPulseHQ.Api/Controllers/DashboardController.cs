using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPulseHQ.Infrastructure.Persistence;

namespace ApiPulseHQ.Api.Controllers
{
    [ApiController]
    [Route("dashboard")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ApiPulseDbContext _db;

        public DashboardController(ApiPulseDbContext db)
        {
            _db = db;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirst("userId")!.Value);

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var userId = GetUserId();

            var endpoints = await _db.ServiceEndpoints.Where(x => x.UserId == userId).ToListAsync();
            var logs = await _db.ServiceCheckLogs
                .Where(x => endpoints.Select(e => e.Id).Contains(x.ServiceEndpointId))
                .ToListAsync();

            int total = logs.Count;
            int success = logs.Count(x => x.IsSuccess);

            return Ok(new
            {
                TotalEndpoints = endpoints.Count,
                TotalChecks = total,
                Uptime = total == 0 ? 0 : Math.Round((double)success / total * 100, 2),
                LastChecked = logs.Any() ? logs.Max(x => x.CheckedAt) : (DateTime?)null
            });
        }
    }
}
