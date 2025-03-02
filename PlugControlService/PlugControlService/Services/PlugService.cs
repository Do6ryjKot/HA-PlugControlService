using Newtonsoft.Json;
using PlugControlService.Configuration;
using PlugControlService.Interfaces;
using PlugControlService.Models;

namespace PlugControlService.Services {

	public class PlugService : IHomeAssistantPlugService {

		private readonly HttpClient _client;
		private readonly Endpoints _endpoints;

		public PlugService(HttpClient client, Endpoints endpoints) {
			_client = client;
			_endpoints = endpoints;
		}

		public async Task<EntityState> TurnOffAsync(Entity plug) {

			var entityStates = await PlugSetStateAsync(_endpoints.TurnOff, plug);
			return entityStates.Length == 0 ? new EntityState { State = "off" } : entityStates[0];
		}

		public async Task<EntityState> TurnOnAsync(Entity plug) {

			var entityStates = await PlugSetStateAsync(_endpoints.TurnOn, plug);
			return entityStates.Length == 0 ? new EntityState { State = "on" } : entityStates[0];
		}

		private async Task<EntityState[]> PlugSetStateAsync(string endpoint, Entity plug) {

			var response = await _client.PostAsJsonAsync(endpoint, new { entity_id = plug.Id });
			var content = await response.Content.ReadAsStringAsync();

			if (!response.IsSuccessStatusCode) {
				throw new Exception($"Error: {response.StatusCode} {content}");
			}

			var entityStates = JsonConvert.DeserializeObject<EntityState[]>(content);

			if (entityStates == null) {
				throw new Exception($"Error while parsing: {content}");
			}

			return entityStates;
		}
	}
}
