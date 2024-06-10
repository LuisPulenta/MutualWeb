using Microsoft.EntityFrameworkCore;
using MutualWeb.Backend.Data;
using MutualWeb.Backend.Repositories.Interfaces;
using MutualWeb.Shared.Entities.Clientes;
using MutualWeb.Shared.Responses;

namespace MutualWeb.Backend.Repositories.Implementations
{
    public class TiposClientesRepository : GenericRepository<TipoCliente>, ITiposClientesRepository
    {
        private readonly DataContext _context;

        public TiposClientesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<IEnumerable<TipoCliente>>> GetAsync()
        {
            var countries = await _context.TipoClientes
                .Include(c => c.Clientes)
                .ToListAsync();
            return new ActionResponse<IEnumerable<TipoCliente>>
            {
                WasSuccess = true,
                Result = countries
            };
        }

        public override async Task<ActionResponse<TipoCliente>> GetAsync(int id)
        {
            var especialidad = await _context.TipoClientes
                 .Include(c => c.Clientes!)
                 .FirstOrDefaultAsync(c => c.Id == id);

            if (especialidad == null)
            {
                return new ActionResponse<TipoCliente>
                {
                    WasSuccess = false,
                    Message = "Tipo de Cliente no existe"
                };
            }

            return new ActionResponse<TipoCliente>
            {
                WasSuccess = true,
                Result = especialidad
            };
        }
    }
}

