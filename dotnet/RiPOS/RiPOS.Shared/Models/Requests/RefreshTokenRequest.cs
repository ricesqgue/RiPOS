using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Models.Requests;

public class RefreshTokenRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "El access token es requerido")]
    public string AccessToken { get; set; } = string.Empty;
}