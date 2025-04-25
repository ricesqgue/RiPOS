using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Models.Requests;

public class AuthRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre de usuario es requerido")]
    public string Username { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "La contraseña es requerida")]
    public string Password { get; set; } = string.Empty;
}