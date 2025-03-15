using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Models.Requests
{
    public class LoginRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre de usuario es requerido")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El código de la compañía es requerida")]
        public string CompanyCode { get; set; }
    }
}
