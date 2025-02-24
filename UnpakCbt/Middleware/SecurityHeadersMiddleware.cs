using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace UnpakCbt.Api.Middleware
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Remove("X-AspNet-Version");
            context.Response.Headers["X-DNS-Prefetch-Control"] = "off";
            context.Response.Headers.XFrameOptions = "DENY";
            context.Response.Headers.XXSSProtection = "1; mode=block";
            context.Response.Headers.XContentTypeOptions = "nosniff";
            context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
            context.Response.Headers.ContentType = "application/json; charset=UTF-8";
            context.Response.Headers.StrictTransportSecurity = "max-age=60; includeSubDomains; preload";
            context.Response.Headers.AccessControlAllowOrigin = Environment.GetEnvironmentVariable("Mode") == "prod" ? "https://host.docker.internal" : "https://localhost";
            context.Response.Headers["Cross-Origin-Opener-Policy"] = "same-origin";
            context.Response.Headers["Cross-Origin-Embedder-Policy"] = "require-corp";
            context.Response.Headers["Cross-Origin-Resource-Policy"] = "same-site";
            context.Response.Headers.Remove("X-Powered-By");
            await _next(context);
        }
    }
}
