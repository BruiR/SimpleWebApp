using Microsoft.AspNetCore.Http.Extensions;
using Serilog;

namespace SimpleWebApp.Middlewares
{
    public class HttpLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            var request = GetRequestAsText(context.Request);
            Log.Information(request);
            await _next.Invoke(context);
        }


        private string GetRequestAsText(HttpRequest request)
        {
            string method = request.Method;
            string url = request.GetDisplayUrl();
            string headers = String.Empty;
            foreach (var key in request.Headers.Keys)
                headers += key + "=" + request.Headers[key] + Environment.NewLine;
            return $"{method} {url} {headers}";
        }

    }
}