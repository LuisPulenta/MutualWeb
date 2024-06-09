using MutualWeb.Shared.Entities.Clientes;

namespace MutualWeb.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckEspecialidadesAsync();
            await CheckTiposClientesAsync();
        }

        private async Task CheckEspecialidadesAsync()
        {
            if (!_context.Especialidades.Any())
            {
                _context.Especialidades.Add(new Especialidad { Nombre = "ALERGIA E INMUNOLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "ALIMENTACION" });
                _context.Especialidades.Add(new Especialidad { Nombre = "ANATOMÍA PATOLÓGICA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "ANESTESIOLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "BIOINGENIERIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "BIOLOGIA MOLECULAR" });
                _context.Especialidades.Add(new Especialidad { Nombre = "CALIDAD DE SERVICIO" });
                _context.Especialidades.Add(new Especialidad { Nombre = "CARDIOLOGÍA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "CIRUGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "CLINICA MEDICA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "CLUB DE LA SALUD" });
                _context.Especialidades.Add(new Especialidad { Nombre = "COBERTURA DE SALUD" });
                _context.Especialidades.Add(new Especialidad { Nombre = "COBRANZAS" });
                _context.Especialidades.Add(new Especialidad { Nombre = "COMPRAS" });
                _context.Especialidades.Add(new Especialidad { Nombre = "CÓMPUTOS" });
                _context.Especialidades.Add(new Especialidad { Nombre = "CONTABILIDAD" });
                _context.Especialidades.Add(new Especialidad { Nombre = "DERMATOLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "DIABETES Y NUTRICION" });
                _context.Especialidades.Add(new Especialidad { Nombre = "DIAGNOSITICO POR IMÁGENES" });
                _context.Especialidades.Add(new Especialidad { Nombre = "DIALISIS PERITONEAL" });
                _context.Especialidades.Add(new Especialidad { Nombre = "DIRECCION" });
                _context.Especialidades.Add(new Especialidad { Nombre = "ECOGRAFIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "ENFERMERÍA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "FACTURACION" });
                _context.Especialidades.Add(new Especialidad { Nombre = "FARMACIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "FONOAUDIOLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "GASTROENTEROLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "GENETICA MEDICA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "GERENCIA MEDICA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "GINECOLOGIA Y OBSTETRICIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "GUARDIA PEDRIATRIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "HEMATOLOGÍA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "HEMODIALISIS" });
                _context.Especialidades.Add(new Especialidad { Nombre = "HEMOTERAPIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "HIGIENE Y SEGURIDAD" });
                _context.Especialidades.Add(new Especialidad { Nombre = "HISTOCOMPATIBILIDAD" });
                _context.Especialidades.Add(new Especialidad { Nombre = "INFECTOLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "INMUNOLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "INTENDENCIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "INTERNADO" });
                _context.Especialidades.Add(new Especialidad { Nombre = "KINESIOLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "LABORATORIO" });
                _context.Especialidades.Add(new Especialidad { Nombre = "MEDICINA AMBULATORA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "MEDICINA FAMILIAR" });
                _context.Especialidades.Add(new Especialidad { Nombre = "MEDICINA LABORAL" });
                _context.Especialidades.Add(new Especialidad { Nombre = "MEDICINA NUCLEAR" });
                _context.Especialidades.Add(new Especialidad { Nombre = "MEDICINA TRANSFUSIONAL" });
                _context.Especialidades.Add(new Especialidad { Nombre = "MEDICINA VASCULAR" });
                _context.Especialidades.Add(new Especialidad { Nombre = "MICROBIOLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "NEFROLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "NEONATOLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "NEUMOLOGÍA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "OBSTETRICIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "ODONTOLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "OFTALMOLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "ORL" });
                _context.Especialidades.Add(new Especialidad { Nombre = "ORTOPEDIA Y TRAUMATOLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "PATOLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "PEDIATRIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "PSICOLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "PSIQUIATRIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "REUMATOLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "TERAPIA FISICA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "TERAPIA INTENSIVA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "TRANSPLANTE" });
                _context.Especialidades.Add(new Especialidad { Nombre = "TRAUMATOLOGIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "UNIDAD CORONARIA" });
                _context.Especialidades.Add(new Especialidad { Nombre = "UROLOGIA" });

            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckTiposClientesAsync()
        {
            if (!_context.TipoClientes.Any())
            {
                _context.TipoClientes.Add(new TipoCliente { DescripcionTipoCliente = "Médicos Staff" ,ClienteInae= "Activo" });
                _context.TipoClientes.Add(new TipoCliente { DescripcionTipoCliente = "Médicos contratados", ClienteInae = "Activo" });
                _context.TipoClientes.Add(new TipoCliente { DescripcionTipoCliente = "Personal a sueldo", ClienteInae = "Activo" });
                _context.TipoClientes.Add(new TipoCliente { DescripcionTipoCliente = "Jubilados", ClienteInae = "Adherente" });
                _context.TipoClientes.Add(new TipoCliente { DescripcionTipoCliente = "Aportantes Externos", ClienteInae = "Activo" });
                _context.TipoClientes.Add(new TipoCliente { DescripcionTipoCliente = "Pasantes", ClienteInae = "Activo" });
                _context.TipoClientes.Add(new TipoCliente { DescripcionTipoCliente = "Pensionados", ClienteInae = "Adherente" });
                _context.TipoClientes.Add(new TipoCliente { DescripcionTipoCliente = "Honor. Admin. (Camilucci)", ClienteInae = "Activo" });
                _context.TipoClientes.Add(new TipoCliente { DescripcionTipoCliente = "Honor. Admin. (Oviedo)", ClienteInae = "Activo" });
            }

            await _context.SaveChangesAsync();
        }
    }
}
