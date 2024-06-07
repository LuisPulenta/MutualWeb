using System.ComponentModel.DataAnnotations;

namespace MutualWeb.Shared.Entities.Clientes
{
    public class Especialidad
    {
        public int Id { get; set; }

        [Display(Name = "Especialidad")]
        [MaxLength(30, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombre { get; set; } = null!;
    }
}
