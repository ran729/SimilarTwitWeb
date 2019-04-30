using Microsoft.AspNetCore.Mvc;

namespace SimilarTwitWeb.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public ActionResult Home()
        {
            return Ok("Welcome to SimilarTwitWeb");
        }
    }
}
