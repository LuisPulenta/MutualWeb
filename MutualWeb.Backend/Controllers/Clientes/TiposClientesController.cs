using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MutualWeb.Backend.UnitsOfWork.Implementations;
using MutualWeb.Backend.UnitsOfWork.Interfaces;
using MutualWeb.Shared.DTOs;
using MutualWeb.Shared.Entities.Clientes;

namespace MutualWeb.Backend.Controllers
{
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class TiposClientesController : GenericController<TipoCliente>
    {
        private readonly ITiposClientesUnitOfWork _tiposClientesUnitOfWork;

        public TiposClientesController(IGenericUnitOfWork<TipoCliente> unit, ITiposClientesUnitOfWork tiposClientesUnitOfWork) : base(unit)
        {
            _tiposClientesUnitOfWork = tiposClientesUnitOfWork;
        }

        //--------------------------------------------------------------------------------------------
        [HttpGet]
        public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
        {
            var response = await _tiposClientesUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        //--------------------------------------------------------------------------------------------
        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _tiposClientesUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        //--------------------------------------------------------------------------------------------
        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _tiposClientesUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }
    }
}
