using MutualWeb.Shared.DTOs;
using MutualWeb.Shared.Entities.Clientes;
using MutualWeb.Shared.Responses;

namespace MutualWeb.Backend.UnitsOfWork.Interfaces
{
    public interface IEspecialidadesUnitOfWork
    {
        Task<ActionResponse<Especialidad>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<Especialidad>>> GetAsync(PaginationDTO pagination);

        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
    }
}

