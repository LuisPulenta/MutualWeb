using Microsoft.EntityFrameworkCore;
using MutualWeb.Shared.Entities.Clientes;

namespace MutualWeb.Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<TipoCliente> TipoClientes { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TipoCliente>().HasIndex(c => c.DescripcionTipoCliente).IsUnique();
            modelBuilder.Entity<Especialidad>().HasIndex(c => c.Nombre).IsUnique();
        }
    }
}

