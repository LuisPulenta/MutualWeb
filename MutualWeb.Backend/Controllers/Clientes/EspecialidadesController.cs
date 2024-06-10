﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MutualWeb.Backend.UnitsOfWork.Interfaces;
using MutualWeb.Shared.Entities.Clientes;

namespace MutualWeb.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class EspecialidadesController : GenericController<Especialidad>
    {
        private readonly IEspecialidadesUnitOfWork _especialidadesUnitOfWork;

        public EspecialidadesController(IGenericUnitOfWork<Especialidad> unit, IEspecialidadesUnitOfWork especialidadesUnitOfWork) : base(unit)
        {
            _especialidadesUnitOfWork = especialidadesUnitOfWork;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            var response = await _especialidadesUnitOfWork.GetAsync();
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _especialidadesUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }
    }
}
