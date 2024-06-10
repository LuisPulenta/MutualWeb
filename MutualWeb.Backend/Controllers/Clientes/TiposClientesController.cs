﻿using Microsoft.AspNetCore.Mvc;
using MutualWeb.Backend.UnitsOfWork.Interfaces;
using MutualWeb.Shared.Entities.Clientes;


namespace MutualWeb.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TiposClientesController : GenericController<TipoCliente>
    {
        private readonly ITiposClientesUnitOfWork _tiposClientesUnitOfWork;

        public TiposClientesController(IGenericUnitOfWork<TipoCliente> unit, ITiposClientesUnitOfWork tiposClientesUnitOfWork) : base(unit)
        {
            _tiposClientesUnitOfWork = tiposClientesUnitOfWork;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            var response = await _tiposClientesUnitOfWork.GetAsync();
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

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
