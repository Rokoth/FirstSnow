using DataBaseEngine.Abstract;
using DataBaseEngine.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FirstSnow.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DbPgContext _context;

        public MapController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _context = serviceProvider.GetRequiredService<DbPgContext>();
        }

        [HttpGet("byPos/{x}/{y}")]
        public async Task<IActionResult> GetByPos([FromRoute] int x, int y)
        {
            var result = await _context.Set<Map>()
                .Include("Position")
                .Include("Fields")
                .Include("Fields.Position")
                .Include("Fields.Properties")
                .Include("Person")
                .Include("Person.Position")
                .Include("Person.PersonProperties")
                .Where(s => s.Position.X == x && s.Position.Y == y)
                .FirstOrDefaultAsync().ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _context.Set<Map>()
                .Include("Position")
                .ToListAsync().ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet("byId/{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _context.Set<Map>()
                .Include("Position")
                .Include("Fields")
                .Include("Fields.Position")
                .Include("Fields.Properties")
                .Include("Person")
                .Include("Person.Position")
                .Include("Person.PersonProperties")
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync().ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet("byPerson/{id}")]
        public async Task<IActionResult> GetByPerson([FromRoute] Guid id)
        {
            var result = await _context.Set<Map>()
                .Include("Position")
                .Include("Fields")
                .Include("Fields.Position")
                .Include("Fields.Properties")
                .Include("Person")
                .Include("Person.Position")
                .Include("Person.PersonProperties")
                .Where(s => s.Persons.Select(t => t.Id).Contains(id))
                .FirstOrDefaultAsync().ConfigureAwait(false);
            return Ok(result);
        }
    }
}