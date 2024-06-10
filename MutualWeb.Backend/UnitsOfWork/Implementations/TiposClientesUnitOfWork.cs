using MutualWeb.Backend.Repositories.Implementations;
using MutualWeb.Backend.Repositories.Interfaces;
using MutualWeb.Backend.UnitsOfWork.Interfaces;
using MutualWeb.Shared.DTOs;
using MutualWeb.Shared.Entities.Clientes;
using MutualWeb.Shared.Responses;

namespace MutualWeb.Backend.UnitsOfWork.Implementations
{
    public class TiposClientesUnitOfWork : GenericUnitOfWork<TipoCliente>, ITiposClientesUnitOfWork
    {
        private readonly ITiposClientesRepository _tiposClientesRepository;

        public TiposClientesUnitOfWork(IGenericRepository<TipoCliente> repository, ITiposClientesRepository tiposClientesRepository) : base(repository)
        {
            _tiposClientesRepository = tiposClientesRepository;
        }

        public override async Task<ActionResponse<IEnumerable<TipoCliente>>> GetAsync(PaginationDTO pagination) => await _tiposClientesRepository.GetAsync(pagination);

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination) => await _tiposClientesRepository.GetTotalPagesAsync(pagination);

        public override async Task<ActionResponse<TipoCliente>> GetAsync(int id) => await _tiposClientesRepository.GetAsync(id);
    }
}

