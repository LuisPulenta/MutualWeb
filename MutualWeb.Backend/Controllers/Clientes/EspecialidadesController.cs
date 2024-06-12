using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MutualWeb.Backend.Data;
using MutualWeb.Backend.Helpers;
using MutualWeb.Shared.DTOs;
using MutualWeb.Shared.Entities.Clientes;
using System.Diagnostics.Metrics;

namespace MutualWeb.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class EspecialidadesController : ControllerBase
    {
        private readonly DataContext _context;

        public EspecialidadesController(DataContext context)
        {
            _context = context;
        }

        //--------------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Especialidades
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Nombre.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return Ok(await queryable
                .OrderBy(x => x.Nombre)
                .Paginate(pagination)
                .ToListAsync());
        }

        //--------------------------------------------------------------------------------------------
        [HttpGet("totalPages")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Especialidades
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Nombre.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        //--------------------------------------------------------------------------------------------
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var category = await _context.Especialidades
                .FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        //--------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult> Post(Especialidad especialidad)
        {
            _context.Add(especialidad);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(especialidad);
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
        public async Task<ActionResult> Put(Especialidad especialidad)
        {
            _context.Update(especialidad);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(especialidad);
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
            var especialidad = await _context.Especialidades.FirstOrDefaultAsync(x => x.Id == id);
            if (especialidad == null)
            {
                return NotFound();
            }

            _context.Remove(especialidad);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //--------------------------------------------------------------------------------------------

        [AllowAnonymous]
        [HttpGet("combo")]
        public async Task<IEnumerable<Especialidad>> GetComboAsync()
        {
            return await _context.Especialidades
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }
    }
}
