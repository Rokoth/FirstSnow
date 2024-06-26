using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FirstSnow.Contract.Interfaces;
using FirstSnow.Contract.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FirstSnow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController(IPersonDataService personDataService, ILogger<PersonController> logger) : ControllerBase
    {
        private IPersonDataService PersonDataService { get; } = personDataService;
        private ILogger<PersonController> Logger { get; } = logger;

        [HttpGet("getlist")]
        public async Task<IActionResult> GetListAsync(PersonFilter filter)
        {
            try
            {
                var userId = Guid.Parse(User.Identity.Name);
                var source = new CancellationTokenSource(30000);
                var result = await PersonDataService.GetAsync(filter, userId, source.Token);
                if (result.IsSuccess)
                    return Ok(result.Value);

                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Ошибка при получении списка персонажей");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}