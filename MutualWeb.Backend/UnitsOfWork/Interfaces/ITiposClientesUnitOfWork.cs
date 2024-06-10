using MutualWeb.Shared.Entities.Clientes;
using MutualWeb.Shared.Responses;

namespace MutualWeb.Backend.UnitsOfWork.Interfaces
{
    public interface ITiposClientesUnitOfWork
    {
        Task<ActionResponse<TipoCliente>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<TipoCliente>>> GetAsync();
    }
}

