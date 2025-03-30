using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Models.Requests
{
    public class LoginRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre de usuario es requerido")]
        public required string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "La contraseña es requerida")]
        public required string Password { get; set; }
    }
}
