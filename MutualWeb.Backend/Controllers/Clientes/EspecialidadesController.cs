using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualWeb.Backend.Data;
using MutualWeb.Shared.Entities.Clientes;

namespace MutualWeb.Backend.Controllers.Clientes
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspecialidadesController : ControllerBase
    {
        private readonly DataContext _context;

        public EspecialidadesController(DataContext context)
        {
            _context = context;
        }

        //-----------------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.Especialidades.ToListAsync());
        }

        //-----------------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var especialidad = await _context.Especialidades.FirstOrDefaultAsync(c => c.Id == id);
            if (especialidad == null)
            {
                return NotFound();
            }

            return Ok(especialidad);
        }

        //-----------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> PostAsync(Especialidad especialidad)
        {
            _context.Add(especialidad);
            await _context.SaveChangesAsync();
            return Ok(especialidad);
        }

        //-----------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var especialidad = await _context.Especialidades.FirstOrDefaultAsync(c => c.Id == id);
            if (especialidad == null)
            {
                return NotFound();
            }

            _context.Remove(especialidad);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //-----------------------------------------------------------------------------
        [HttpPut]
        public async Task<IActionResult> PutAsync(Especialidad especialidad)
        {
            _context.Update(especialidad);
            await _context.SaveChangesAsync();
            return Ok(especialidad);
        }
    }
}

