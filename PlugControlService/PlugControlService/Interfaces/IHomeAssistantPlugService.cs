using PlugControlService.Models;

namespace PlugControlService.Interfaces {
	
	/// <summary>
	/// Controls entity state
	/// </summary>
	public interface IHomeAssistantPlugService {

		/// <summary>
		/// Turns on the entity
		/// </summary>
		/// <param name="entity">Entity to turn on</param>
		public Task<EntityState> TurnOnAsync(Entity entity);

		/// <summary>
		/// Turns off the entity
		/// </summary>
		/// <param name="entity">Entity to turn off</param>
		public Task<EntityState> TurnOffAsync(Entity entity);
	}
}
