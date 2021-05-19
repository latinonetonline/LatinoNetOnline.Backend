using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System.Net;
using System.Text.Json;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.StartupExtensions
{
    public static class ExceptionHandlingExceptions
    {
        public static IApplicationBuilder UseExceptionHandlingOperationResult(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature is not null)
                    {
                        logger.LogError(contextFeature.Error, contextFeature.Error.ToString());

                        ErrorResult errorResult = new("internal_server_error");

                        await context.Response.WriteAsync(JsonSerializer.Serialize(OperationResult.ServerError(errorResult)));
                    }
                });
            });
            return app;
        }
    }
}
