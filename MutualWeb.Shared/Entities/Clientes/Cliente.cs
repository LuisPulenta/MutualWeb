using System.ComponentModel.DataAnnotations;

namespace MutualWeb.Shared.Entities.Clientes
{
    public class Cliente
    {
        public int Id { get; set; }

        [Display(Name = "Apellido Titular")]
        [MaxLength(25, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ApellidoTitular { get; set; } = null!;

        [Display(Name = "Nombre Titular")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string NombreTitular { get; set; } = null!;

        [MaxLength(25, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Apellido { get; set; } = null!;

        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int DNI { get; set; }

        public string? CUIL { get; set; }

        [MaxLength(1, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string? Sexo { get; set; } = null!;

        [Display(Name = "Fecha de Nacimiento")]
        public DateOnly? FechaNacimiento { get; set; }

        [Display(Name = "Fecha de Fallecimiento")]
        public DateOnly? FechaFallecimiento { get; set; }

        [Display(Name = "Estado Civil")]
        [MaxLength(3, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string EstadoCivil { get; set; } = null!;

        [Display(Name = "Lugar de Nacimiento")]
        [MaxLength(25, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? LugarNacimiento { get; set; }

        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? Nacionalidad { get; set; }

        [MaxLength(70, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? Domicilio { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? Barrio { get; set; }

        [MaxLength(25, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? Localidad { get; set; }

        public int? CP { get; set; }

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? Telefono { get; set; }

        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? Celular { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? Email { get; set; }

        [Display(Name = "Apellido Cónyuge")]
        [MaxLength(25, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string? ApellidoConyuge { get; set; } = null!;

        [Display(Name = "Nombre Cónyuge")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string? NombreConyuge { get; set; } = null!;

        [Display(Name = "DNI Cónyuge")]
        public int? DNIConyuge { get; set; }

        [Display(Name = "CUIL Cónyuge")]
        public string? CUILConyuge { get; set; }

        [Display(Name = "Fecha de Nacimiento Cónyuge")]
        public DateOnly? FechaNacimientoConyuge { get; set; }

        [Display(Name = "Fecha de Fallecimiento Cónyuge")]
        public DateOnly? FechaFallecimientoConyuge { get; set; }

        [Display(Name = "Tipo de Cliente")]
        public TipoCliente TipoCliente { get; set; } = null!;

        public bool Socio { get; set; }

        [Display(Name = "Fecha de Carga")]
        public DateOnly? FechaCarga { get; set; }

        [Display(Name = "Fecha de Alta")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateOnly? FechaAlta { get; set; }

        [Display(Name = "Fecha de Baja")]
        public DateOnly? FechaBaja { get; set; }

        [Display(Name = "Motivo de Baja")]
        [MaxLength(255, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? MotivoBaja { get; set; }

        [Display(Name = "N° de Socio")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? NroSocio { get; set; }

        [Display(Name = "N° de Cuenta HP")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? NroCuentaHP { get; set; }

        [Display(Name = "Tipo de Jubilación")]
        [MaxLength(15, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? TipoJubilacion { get; set; }

        [Display(Name = "Fecha de Jubilación")]
        public DateOnly? FechaJubilacion { get; set; }

        [Display(Name = "Especialidad")]
        public Especialidad? Especialidad { get; set; }

        [Display(Name = "Especialidad Id")]
        public int? EspecialidadId { get; set; }

        [Display(Name = "Af. Seguro Titular")]
        public bool? AfSeguroTit { get; set; }

        [Display(Name = "Af. Seguro Cónyuge")]
        public bool? AfSeguroCony { get; set; }

        [Display(Name = "Trabaja en HP")]
        public bool? TrabHP { get; set; }

        [Display(Name = "Trabaja fuera del HP")]
        public bool? TrabFuera { get; set; }

        [Display(Name = "Lugar de Trabajo")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? LugarTrab { get; set; }

        [Display(Name = "Seguro Enfermedad")]
        public bool? SegEnf { get; set; }

        [Display(Name = "Seguro Accidente")]
        public bool? SegAcc { get; set; }

        [Display(Name = "Beneficio")]
        public bool? Beneficio1 { get; set; }

        public DateOnly? Supervivencia { get; set; }

        [Display(Name = "Fecha Pago Sepelio Titular")]
        public DateOnly? FecPagoSepTitular { get; set; }

        [Display(Name = "Egreso Pago Sepelio Titular")]
        public int? EgresoPagoSepTitular { get; set; }

        [Display(Name = "Monto Pago Sepelio Titular")]
        public float? MontoPagoSepTitular { get; set; }

        [Display(Name = "Fecha Pago Sepelio Cónyuge")]
        public DateOnly? FecPagoSepConyuge { get; set; }

        [Display(Name = "Egreso Pago Sepelio Cónyuge")]
        public int? EgresoPagoSepConyuge { get; set; }

        [Display(Name = "Monto Pago Sepelio Cónyuge")]
        public float? MontoPagoSepConyuge { get; set; }

        [Display(Name = "Fecha Pago Seguro Enfermedad")]
        public DateOnly? FecPagoSegEnf { get; set; }

        [Display(Name = "Egreso Pago Seguro Enfermedad")]
        public int? EgresoPagoSegEnf { get; set; }

        [Display(Name = "Monto Pago Seguro Enfermedad")]
        public float? MontoPagoSegEnf { get; set; }

        [Display(Name = "Plan Seguro Enfermedad")]
        [MaxLength(1, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? PlanSegEnf { get; set; }

        [Display(Name = "Observaciones Seguro Enfermedad")]
        [MaxLength(255, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? ObsSegEnfAcc { get; set; }
    }
}
