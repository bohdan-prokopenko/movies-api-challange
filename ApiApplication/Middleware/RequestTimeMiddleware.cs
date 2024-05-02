using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ApiApplication.Middleware {
    public class RequestTimeMiddleware {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimeMiddleware> _logger;

        public RequestTimeMiddleware(RequestDelegate next, ILogger<RequestTimeMiddleware> logger) {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context) {
            var watch = Stopwatch.StartNew();
            await _next(context);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            string path = context.Request.Path;
            var requestMethod = context.Request.Method;
            _logger.LogInformation($"({requestMethod}) Request to {path} completed in {elapsedMs} ms.");
        }
    }
}
