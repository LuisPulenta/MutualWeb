using MutualWeb.Shared.Entities;
using System.ComponentModel.DataAnnotations;

namespace MutualWeb.Shared.DTOs
{
    public class UserDTO : User
    {

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int RolId { get; set; }

        public string Password { get; set; } = null!;

        
    }
}
