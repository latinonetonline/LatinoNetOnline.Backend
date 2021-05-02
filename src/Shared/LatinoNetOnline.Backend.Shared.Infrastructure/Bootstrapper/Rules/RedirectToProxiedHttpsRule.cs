﻿
using Microsoft.AspNetCore.Rewrite;

using System.Text;

namespace LatinoNetOnline.Backend.Shared.Infrastructure.Bootstrapper.Rules
{
    public class RedirectToProxiedHttpsRule : IRule
    {
        public virtual void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;

            // #1) Did this request start off as HTTP?
            string reqProtocol;
            if (request.Headers.ContainsKey("X-Forwarded-Proto"))
            {
                reqProtocol = request.Headers["X-Forwarded-Proto"][0];
            }
            else
            {
                reqProtocol = request.IsHttps ? "https" : "http";
            }


            // #2) If so, redirect to HTTPS equivalent
            if (reqProtocol is not "https")
            {
                StringBuilder newUrl = new();

                newUrl.Append("https://").Append(request.Host)
                    .Append(request.PathBase).Append(request.Path)
                    .Append(request.QueryString);

                context.HttpContext.Response.Redirect(newUrl.ToString(), true);
            }
        }
    }
}
