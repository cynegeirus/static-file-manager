using Microsoft.AspNetCore.Mvc;

namespace StaticFileManager.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WelcomeController(ILogger<WelcomeController> logger) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
