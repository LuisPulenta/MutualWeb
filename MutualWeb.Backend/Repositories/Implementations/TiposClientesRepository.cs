using Microsoft.EntityFrameworkCore;
using MutualWeb.Backend.Data;
using MutualWeb.Backend.Helpers;
using MutualWeb.Backend.Repositories.Interfaces;
using MutualWeb.Shared.DTOs;
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

        //-------------------------------------------------------------------------------------------------
        public override async Task<ActionResponse<IEnumerable<TipoCliente>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.TipoClientes
             .Include(c => c.Clientes)
             .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.DescripcionTipoCliente.ToLower().Contains(pagination.Filter.ToLower()));
            }


            return new ActionResponse<IEnumerable<TipoCliente>>
            {
                WasSuccess = true,
                Result = await queryable
                .Paginate(pagination)
                .ToListAsync()
            };
        }

        //-------------------------------------------------------------------------------------------------
        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.TipoClientes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.DescripcionTipoCliente.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = totalPages
            };
        }

        //-------------------------------------------------------------------------------------------------
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

