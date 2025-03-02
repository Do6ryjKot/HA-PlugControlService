using PlugControlService.Extensions;
using PlugControlService.Interfaces;
using PlugControlService.Services;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureCors();

builder.Services.AddHttpClient<IHomeAssistantPlugService, PlugService>().ConfigureHttpClient(client => {

	client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("HA_API_BASE_URL") 
		?? throw new Exception("HA_API_BASE_URL not set"));
	client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("HA_KEY") 
		?? throw new Exception("HA_KEY not set"));
});

builder.Services.ConfigureHomeAssistantEndpoints();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

// app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.ConfigureExceptionHandler();
app.MapControllers();
app.Run();
