using Microsoft.EntityFrameworkCore;
using MutualWeb.Backend.Data;
using MutualWeb.Backend.Helpers;
using MutualWeb.Backend.Repositories.Interfaces;
using MutualWeb.Shared.DTOs;
using MutualWeb.Shared.Entities.Clientes;
using MutualWeb.Shared.Responses;

namespace MutualWeb.Backend.Repositories.Implementations
{
    public class EspecialidadesRepository : GenericRepository<Especialidad>, IEspecialidadesRepository
    {
        private readonly DataContext _context;

        public EspecialidadesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        //-------------------------------------------------------------------------------------------------
        public override async Task<ActionResponse<IEnumerable<Especialidad>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.Especialidades
            .Include(c => c.Clientes)
            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Nombre.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<Especialidad>>
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
            var queryable = _context.Especialidades.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Nombre.ToLower().Contains(pagination.Filter.ToLower()));
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
        public override async Task<ActionResponse<Especialidad>> GetAsync(int id)
        {
            var especialidad = await _context.Especialidades
                 .Include(c => c.Clientes!)
                 .FirstOrDefaultAsync(c => c.Id == id);

            if (especialidad == null)
            {
                return new ActionResponse<Especialidad>
                {
                    WasSuccess = false,
                    Message = "Especialidad no existe"
                };
            }

            return new ActionResponse<Especialidad>
            {
                WasSuccess = true,
                Result = especialidad
            };
        }
    }
}

