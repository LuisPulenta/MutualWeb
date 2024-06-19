using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualWeb.Backend.Data;
using MutualWeb.Backend.Helpers;
using MutualWeb.Shared.DTOs;
using MutualWeb.Shared.Entities.Clientes;

namespace MutualWeb.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class TiposClientesController : ControllerBase
    {
        private readonly DataContext _context;

        public TiposClientesController(DataContext context)
        {
            _context = context;
        }

        //--------------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.TipoClientes
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.DescripcionTipoCliente.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return Ok(await queryable
                .OrderBy(x => x.DescripcionTipoCliente)
                .Paginate(pagination)
                .ToListAsync());
        }

        //--------------------------------------------------------------------------------------------
        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.TipoClientes
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.DescripcionTipoCliente.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        //--------------------------------------------------------------------------------------------
        [HttpGet("totalRegisters")]
        public async Task<ActionResult> GetTotalRegisters([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.TipoClientes
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.DescripcionTipoCliente.ToLower().Contains(pagination.Filter.ToLower()));
            }

            int count = await queryable.CountAsync();
            return Ok(count);
        }

        //--------------------------------------------------------------------------------------------
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var category = await _context.TipoClientes
                .FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        //--------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult> Post(TipoCliente tipocliente)
        {
            _context.Add(tipocliente);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(tipocliente);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplica"))
                {
                    return BadRequest("Ya existe un registro con el mismo nombre.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        //--------------------------------------------------------------------------------------------
        [HttpPut]
        public async Task<ActionResult> Put(TipoCliente tipocliente)
        {
            _context.Update(tipocliente);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(tipocliente);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException!.Message.Contains("duplica"))
                {
                    return BadRequest("Ya existe un registro con el mismo nombre.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        //--------------------------------------------------------------------------------------------
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var tipocliente = await _context.TipoClientes.FirstOrDefaultAsync(x => x.Id == id);
            if (tipocliente == null)
            {
                return NotFound();
            }

            _context.Remove(tipocliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //--------------------------------------------------------------------------------------------
        [AllowAnonymous]
        [HttpGet("combo")]
        public async Task<IEnumerable<TipoCliente>> GetComboAsync()
        {
            return await _context.TipoClientes
                .OrderBy(c => c.DescripcionTipoCliente)
                .ToListAsync();
        }
    }
}
