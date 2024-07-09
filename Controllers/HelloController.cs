using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutomaticProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public async  Task<IActionResult> GetStatus()
        {
            var html = "<html><body><h1>Funcionando</h1></body></html>";
            return Content(html, "text/html");
        }
    }
}