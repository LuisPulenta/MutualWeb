using Microsoft.AspNetCore.Mvc;
using MutualWeb.Backend.UnitsOfWork.Interfaces;
using MutualWeb.Shared.Entities.Clientes;


namespace MutualWeb.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TiposClientesController : GenericController<TipoCliente>
    {
        public TiposClientesController(IGenericUnitOfWork<TipoCliente> unit) : base(unit)
        {
        }
    }
}
