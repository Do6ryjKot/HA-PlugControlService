using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;

namespace PlugControlService.Extensions {

    public static class ExceptionMiddlewareExtensions {

		public static void ConfigureExceptionHandler(this IApplicationBuilder app) =>
			app.UseExceptionHandler(appError => appError.Run(async context => {

				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				context.Response.ContentType = "application/json";

				var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
				if (contextFeature != null) {

					var response = new {
						context.Response.StatusCode,
						contextFeature.Error.Message // "Internal Server Error."
					}; 

					await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
				}
			}));
	}
}
