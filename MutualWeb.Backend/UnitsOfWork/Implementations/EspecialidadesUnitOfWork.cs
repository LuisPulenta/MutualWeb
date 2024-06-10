using MutualWeb.Backend.Repositories.Interfaces;
using MutualWeb.Backend.UnitsOfWork.Interfaces;
using MutualWeb.Shared.DTOs;
using MutualWeb.Shared.Entities.Clientes;
using MutualWeb.Shared.Responses;

namespace MutualWeb.Backend.UnitsOfWork.Implementations
{
    public class EspecialidadesUnitOfWork : GenericUnitOfWork<Especialidad>, IEspecialidadesUnitOfWork
    {
        private readonly IEspecialidadesRepository _especialidadesRepository;

        public EspecialidadesUnitOfWork(IGenericRepository<Especialidad> repository, IEspecialidadesRepository especialidadesRepository) : base(repository)
        {
            _especialidadesRepository = especialidadesRepository;
        }

        public override async Task<ActionResponse<IEnumerable<Especialidad>>> GetAsync(PaginationDTO pagination) => await _especialidadesRepository.GetAsync(pagination);

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _especialidadesRepository.GetTotalPagesAsync(pagination);

        public override async Task<ActionResponse<Especialidad>> GetAsync(int id) => await _especialidadesRepository.GetAsync(id);
    }
}

