
using LatinoNetOnline.Backend.Shared.Commons.OperationResults;

using Microsoft.AspNetCore.Http;

using System;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Middlewares
{
    public class AuthenticationOperationResultMiddleware
    {
        private readonly RequestDelegate _request;

        public AuthenticationOperationResultMiddleware(RequestDelegate RequestDelegate)
        {
            _request = RequestDelegate ?? throw new ArgumentNullException(nameof(RequestDelegate));
        }

        public async Task InvokeAsync(HttpContext Context)
        {
            await _request(Context);

            if (Context.Response.StatusCode == 401)
            {
                await Context.Response.WriteAsJsonAsync(OperationResult.Unauthorized());
            }
        }
    }
}