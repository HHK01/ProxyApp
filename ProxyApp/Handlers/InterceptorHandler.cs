using ProxyApp.Models;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace ProxyApp.Handlers
{
    public class InterceptorHandler
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<List<ProxyApp.Model.Endpoint>> config;

        public InterceptorHandler(RequestDelegate next, IOptions<List<ProxyApp.Model.Endpoint>> config)
        {
            _next = next;
            this.config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            string xsource = context.Request.Headers["X-SOURCE"];
            if (!string.IsNullOrEmpty(context.Request.Headers["X-SOURCE"]))
            {
                var _client = new HttpClient();

                // Get the actual backend url 
                string backendUrl = config.Value.FirstOrDefault(p => p.Name == xsource).BackendBaseEndpoint;
                var request = ProxyHandler.CreateProxyHttpRequest(context, new Uri(backendUrl));
                var response = await _client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);
                await ProxyHandler.CopyProxyHttpResponse(context, response);
            }
            else
                // EXPLAIN HERE,EXPLAIN HERE,EXPLAIN HERE,EXPLAIN HERE,EXPLAIN HERE,EXPLAIN HERE
                await _next(context);
        }
    }
}
