using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualWeb.Backend.Data;
using MutualWeb.Shared.Entities.Clientes;

namespace MutualWeb.Backend.Controllers.Clientes
{
    [ApiController]
    [Route("api/[controller]")]
    public class TiposClientesController : ControllerBase
    {
        private readonly DataContext _context;

        public TiposClientesController(DataContext context)
        {
            _context = context;
        }

        //-----------------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.TipoClientes.ToListAsync());
        }

        //-----------------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var country = await _context.TipoClientes.FirstOrDefaultAsync(c => c.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        //-----------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> PostAsync(TipoCliente tipoCliente)
        {
            _context.Add(tipoCliente);
            await _context.SaveChangesAsync();
            return Ok(tipoCliente);
        }

        //-----------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var tipoCliente = await _context.TipoClientes.FirstOrDefaultAsync(c => c.Id == id);
            if (tipoCliente == null)
            {
                return NotFound();
            }

            _context.Remove(tipoCliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //-----------------------------------------------------------------------------
        [HttpPut]
        public async Task<IActionResult> PutAsync(TipoCliente tipoCliente)
        {
            _context.Update(tipoCliente);
            await _context.SaveChangesAsync();
            return Ok(tipoCliente);
        }
    }
}

