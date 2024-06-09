using MutualWeb.Backend.Repositories.Interfaces;
using MutualWeb.Backend.UnitsOfWork.Interfaces;
using MutualWeb.Shared.Entities.Clientes;
using MutualWeb.Shared.Responses;

namespace MutualWeb.Backend.UnitsOfWork.Implementations
{
    public class EspecialidadesUnitOfWork : GenericUnitOfWork<Especialidad>, IEspecialidadesUnitOfWork
    {
        private readonly IEspecialidadesRepository _countriesRepository;

        public EspecialidadesUnitOfWork(IGenericRepository<Especialidad> repository, IEspecialidadesRepository countriesRepository) : base(repository)
        {
            _countriesRepository = countriesRepository;
        }

        public override async Task<ActionResponse<IEnumerable<Especialidad>>> GetAsync() => await _countriesRepository.GetAsync();

        public override async Task<ActionResponse<Especialidad>> GetAsync(int id) => await _countriesRepository.GetAsync(id);
    }
}

