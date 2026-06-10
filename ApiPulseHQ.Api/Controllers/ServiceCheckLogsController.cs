using ApiPulseHQ.Api.Services.ServiceCheckLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPulseHQ.Api.Controllers
{
    [ApiController]
    [Route("service-endpoints/{endpointId:guid}/logs")]
    [Authorize]
    public class ServiceCheckLogsController : ControllerBase
    {
        private readonly IServiceCheckLogsService _service;

        public ServiceCheckLogsController(IServiceCheckLogsService service)
        {
            _service = service;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirst("userId")!.Value);

        [HttpGet]
        public async Task<IActionResult> GetLogs(Guid endpointId)
        {
            var result = await _service.GetLogsAsync(endpointId, GetUserId());
            return Ok(result);
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest(Guid endpointId)
        {
            var result = await _service.GetLatestAsync(endpointId, GetUserId());
            return Ok(result);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary(Guid endpointId)
        {
            var result = await _service.GetSummaryAsync(endpointId, GetUserId());
            return Ok(result);
        }
    }
}
