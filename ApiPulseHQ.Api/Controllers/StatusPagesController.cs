using ApiPulseHQ.Api.Models.StatusPages;
using ApiPulseHQ.Api.Services.StatusPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPulseHQ.Api.Controllers
{
    [ApiController]
    [Route("status-pages")]
    [Authorize] // 🔒 Protected
    public class StatusPagesController : ControllerBase
    {
        private readonly IStatusPagesService _service;

        public StatusPagesController(IStatusPagesService service)
        {
            _service = service;
        }

        private Guid GetUserId()
        {
            var userId = User.FindFirst("userId")?.Value;
            return Guid.Parse(userId!);
        }

        // GET: /status-pages
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var result = await _service.GetAllAsync(userId);
            return Ok(result);
        }

        // GET: /status-pages/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = GetUserId();
            var result = await _service.GetByIdAsync(userId, id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST: /status-pages
        [HttpPost]
        public async Task<IActionResult> Create(CreateStatusPageRequest request)
        {
            var userId = GetUserId();
            var result = await _service.CreateAsync(userId, request);
            return Ok(result);
        }

        // PUT: /status-pages/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateStatusPageRequest request)
        {
            var userId = GetUserId();
            var result = await _service.UpdateAsync(userId, id, request);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // DELETE: /status-pages/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetUserId();
            var success = await _service.DeleteAsync(userId, id);

            if (!success)
                return NotFound();

            return Ok(new { message = "Deleted successfully" });
        }
    }
}
