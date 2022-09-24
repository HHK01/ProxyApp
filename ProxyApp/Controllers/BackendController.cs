using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProxyApp.Model;
using ProxyApp.Models;
using System.Reflection;

namespace ProxyApp.Controllers
{
    /// <summary>
    /// This class will help us fetch the swagger definition of our backend apis and display them in the Swagger UI index page
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BackendController : ControllerBase
    {
        private readonly IOptions<List<ProxyApp.Model.Endpoint>> config;
        private readonly HttpClient _client;
        public BackendController(IOptions<List<ProxyApp.Model.Endpoint>> config)
        {
            this.config = config;
            _client = new HttpClient(new HttpClientHandler()
            {
                AllowAutoRedirect = false
            });
        }

        [HttpGet]
        public async Task<IActionResult> Endpoint1()
        {
            HttpResponseMessage response = await _client.GetAsync(config.Value.FirstOrDefault(p => p.Name == "Endpoint1").BackendSwaggerEndpoint);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(responseBody);

            // Change the host to the host of the current running application, so that the requests initiated in the Swagger UI be routed back to this application and handled by the Interceptor
            json["host"] = Request.Host.ToString();

            // Add any supported schemes, for localhost in our application It's going to be http only
            List<string> supportedSchemes = new List<string>();
            supportedSchemes.Add(Request.Scheme);
            json["schemes"] = JToken.FromObject(supportedSchemes) ;

            // Add a header parameter called "X-SOURCE" and set the value to the endpoint name, we will use this changge in the "InterceptorHandler.cs"
            JToken paths = json["paths"];
            foreach (JProperty item in paths.Children())
            {
                try
                {
                    IEnumerable<JToken> parameters = item.Children().Children().Children()["parameters"];
                    JArray jParameters = parameters.FirstOrDefault().Value<JArray>();

                    Parameter source = new Parameter();
                    source.name = "X-SOURCE";
                    source.@default = "Endpoint1";
                    source.@in = "header";
                    source.schema = new Schema
                    {
                        type = "string",
                        format = "string",
                        required = "true",
                    };

                    jParameters.Add(JObject.FromObject(source));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return Ok(json.ToString());
        }

        [HttpGet]
        public async Task<IActionResult> Endpoint2()
        {
            HttpResponseMessage response = await _client.GetAsync(config.Value.FirstOrDefault(p => p.Name == "Endpoint1").BackendSwaggerEndpoint);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(responseBody);
            JToken paths = json["paths"];
            foreach (JProperty item in paths.Children())
            {
                try
                {
                    IEnumerable<JToken> parameters = item.Children().Children().Children()["parameters"];
                    JArray jParameters = parameters.FirstOrDefault().Value<JArray>();

                    Parameter source = new Parameter();
                    source.name = "X-SOURCE";

                    // REVIEW THIS, REVIEW THIS, REVIEW THIS, REVIEW THIS, REVIEW THIS, REVIEW THIS, REVIEW THIS
                    //  source.@default = "Endpoint2";
                    //  source.@in = "header";
                    //  source.schema = new Schema
                    //  {
                    //      type = "string",
                    //      format = "string",
                    //      required = "true",
                    //  };
                    // 
                    jParameters.Add(JObject.FromObject(source));
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Ok(json.ToString());
        }
    }
}
