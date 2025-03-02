using PlugControlService.Configuration;

namespace PlugControlService.Extensions {
	
	public static class ServicesExtensions {

		public static void ConfigureHomeAssistantEndpoints(this IServiceCollection services) =>
			services.AddScoped(_ => new Endpoints { 
				TurnOn = Environment.GetEnvironmentVariable("HA_API_ENDP_TURNON") ?? 
					throw new Exception("HA_API_ENDP_TURNON not set"),
				TurnOff = Environment.GetEnvironmentVariable("HA_API_ENDP_TURNOFF") ??
					throw new Exception("HA_API_ENDP_TURNOFF not set")
			});

		public static void ConfigureCors(this IServiceCollection services) =>
			services.AddCors(opts =>
				opts.AddPolicy("CorsPolicy", builder =>
					builder.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader()));
	}
}
