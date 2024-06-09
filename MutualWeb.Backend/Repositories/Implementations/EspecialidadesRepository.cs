using Microsoft.EntityFrameworkCore;
using MutualWeb.Backend.Data;
using MutualWeb.Backend.Repositories.Interfaces;
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

        public override async Task<ActionResponse<IEnumerable<Especialidad>>> GetAsync()
        {
            var countries = await _context.Especialidades
                .Include(c => c.Clientes)
                .ToListAsync();
            return new ActionResponse<IEnumerable<Especialidad>>
            {
                WasSuccess = true,
                Result = countries
            };
        }

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

