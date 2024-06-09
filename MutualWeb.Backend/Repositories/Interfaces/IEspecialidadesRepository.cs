using MutualWeb.Shared.Entities.Clientes;
using MutualWeb.Shared.Responses;

namespace MutualWeb.Backend.Repositories.Interfaces
{
    public interface IEspecialidadesRepository
    {
        Task<ActionResponse<Especialidad>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<Especialidad>>> GetAsync();
    }
}
