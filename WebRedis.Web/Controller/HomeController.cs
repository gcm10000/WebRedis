using Microsoft.AspNetCore.Mvc;

namespace WebRedis.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Produces("application/json")]
        public IActionResult Index()
        {
            return Ok("Servidor aberto!");
        }
    }
}
