using System.ComponentModel.DataAnnotations;

namespace MutualWeb.Shared.Entities.Clientes
{
    public class TipoCliente
    {
        [Display(Name = "Tipo Cliente")]
        public int Id { get; set; }

        [Display(Name = "Descripción Tipo Cliente")]
        [MaxLength(40, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string DescripcionTipoCliente { get; set; } = null!;

        [Display(Name = "Cliente Inae")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string ClienteInae { get; set; } = null!;
    }
}

