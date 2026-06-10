using ApiPulseHQ.Api.Models.AlertRules;
using ApiPulseHQ.Api.Services.AlertRules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPulseHQ.Api.Controllers
{
    [ApiController]
    [Route("alert-rules")]
    [Authorize]
    public class AlertRulesController : ControllerBase
    {
        private readonly IAlertRulesService _service;

        public AlertRulesController(IAlertRulesService service)
        {
            _service = service;
        }

        private Guid GetUserId() =>
            Guid.Parse(User.FindFirst("userId")!.Value);

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync(GetUserId());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAlertRuleRequest request)
        {
            var result = await _service.CreateAsync(GetUserId(), request);

            if (result == null)
                return BadRequest(new { message = "Invalid service endpoint" });

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(GetUserId(), id);

            if (!success)
                return NotFound();

            return Ok(new { message = "Alert rule deleted" });
        }
    }
}
