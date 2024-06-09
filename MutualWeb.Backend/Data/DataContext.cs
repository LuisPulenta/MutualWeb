using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MutualWeb.Shared.Entities;
using MutualWeb.Shared.Entities.Clientes;

namespace MutualWeb.Backend.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<TipoCliente> TipoClientes { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TipoCliente>().HasIndex(c => c.DescripcionTipoCliente).IsUnique();
            modelBuilder.Entity<Especialidad>().HasIndex(c => c.Nombre).IsUnique();
        }

        private void DisableCascadingDelete(ModelBuilder modelBuilder)
        {
            var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
            foreach (var relationship in relationships)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

    }
}

