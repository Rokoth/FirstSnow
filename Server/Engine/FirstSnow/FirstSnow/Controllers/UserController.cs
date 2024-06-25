using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FirstSnow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync(
            [FromQuery]int size = 10,
            [FromQuery]int page = 0,
            [FromQuery]string sort = "Name",
            [FromQuery]string name = null,
            [FromQuery]string server = null
            )
        {
            return Ok();
        }
    }
}