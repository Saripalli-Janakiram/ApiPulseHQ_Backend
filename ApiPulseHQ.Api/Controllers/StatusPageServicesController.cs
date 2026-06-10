using ApiPulseHQ.Api.Models.StatusPages;
using ApiPulseHQ.Api.Services.StatusPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPulseHQ.Api.Controllers
{
    [ApiController]
    [Route("status-pages/{pageId:guid}/services")]
    [Authorize]
    public class StatusPageServicesController : ControllerBase
    {
        private readonly IStatusPageServicesService _service;

        public StatusPageServicesController(IStatusPageServicesService service)
        {
            _service = service;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirst("userId")!.Value);

        [HttpGet]
        public async Task<IActionResult> GetAll(Guid pageId)
        {
            return Ok(await _service.GetAllAsync(pageId, GetUserId()));
        }

        [HttpPost]
        public async Task<IActionResult> Add(Guid pageId, StatusPageServiceRequest request)
        {
            var result = await _service.AddAsync(pageId, GetUserId(), request);
            if (result == null)
                return BadRequest("Invalid status page");

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Remove(Guid pageId, Guid id)
        {
            var success = await _service.RemoveAsync(pageId, GetUserId(), id);
            return success ? Ok() : NotFound();
        }
    }
}
