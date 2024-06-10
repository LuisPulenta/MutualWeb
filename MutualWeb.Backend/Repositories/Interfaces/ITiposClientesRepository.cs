using MutualWeb.Shared.DTOs;
using MutualWeb.Shared.Entities.Clientes;
using MutualWeb.Shared.Responses;

namespace MutualWeb.Backend.Repositories.Interfaces
{
    public interface ITiposClientesRepository
    {
        Task<ActionResponse<TipoCliente>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<TipoCliente>>> GetAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}
