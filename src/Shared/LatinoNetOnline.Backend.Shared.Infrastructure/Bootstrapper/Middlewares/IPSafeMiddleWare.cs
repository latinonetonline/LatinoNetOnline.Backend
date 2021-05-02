
using Microsoft.AspNetCore.Http;

using System.Net;
using System.Threading.Tasks;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Middlewares
{
    public class IPSafeMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly string[] _ipBlackList = { "127.0.0.1", "::1" };

        public IPSafeMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestIpAdress = context.Connection.RemoteIpAddress;

            bool isBlackList = false;

            for (int i = 0; i < _ipBlackList.Length; i++)
            {
                if (IPAddress.Parse(_ipBlackList[i]).Equals(requestIpAdress))
                {
                    isBlackList = true;
                    break;
                }
            }

            if (isBlackList)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            await _next(context);
        }
    }
}
