using Microsoft.AspNetCore.Mvc;
using PlugControlService.Interfaces;
using PlugControlService.Models;
using PlugControlService.Models.Dto;

namespace PlugControlService.Controllers {

	[Route("/api/plug")]
	[ApiController]
	public class PlugController : ControllerBase {

		private readonly IHomeAssistantPlugService _plugService;

		public PlugController(IHomeAssistantPlugService plugService) {
			_plugService = plugService;
		}

		[HttpPost("set-state")]
		public async Task<IActionResult> SetStateAsync([FromBody] EntityStateDto entityStateDto) {

			Entity entity = new() { Id = entityStateDto.EntityId };
			EntityState result;

			if (entityStateDto.State == "on") {
			
				 result = await _plugService.TurnOnAsync(entity);
			
			} else if (entityStateDto.State == "off") {

				 result = await _plugService.TurnOffAsync(entity);
			
			} else 
				return BadRequest();

			return Ok(new EntityStateDto { EntityId = entity.Id, State = result.State });
		}
	}
}
