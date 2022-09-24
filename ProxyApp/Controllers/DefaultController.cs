using Microsoft.AspNetCore.Mvc;

namespace ProxyApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DefaultController : ControllerBase
    {
  
        private readonly ILogger<DefaultController> _logger;

        public DefaultController(ILogger<DefaultController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Default")]
        public string Get()
        {
            return "I am coming the Default controller not the backend swagger endpoints :)";
        }
    }
}