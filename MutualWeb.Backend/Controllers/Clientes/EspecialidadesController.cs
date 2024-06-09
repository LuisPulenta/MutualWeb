using Microsoft.AspNetCore.Mvc;
using MutualWeb.Backend.UnitsOfWork.Interfaces;
using MutualWeb.Shared.Entities.Clientes;


namespace MutualWeb.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspecialidadesController : GenericController<Especialidad>
    {
        public EspecialidadesController(IGenericUnitOfWork<Especialidad> unit) : base(unit)
        {
        }
    }
}
