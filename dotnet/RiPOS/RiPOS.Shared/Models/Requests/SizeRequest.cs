using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Models.Requests;

public class SizeRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre de la talla es requerido")]
    [MaxLength(50, ErrorMessage = "El nombre debe ser de máximo de {1} caracteres")]
    public string Name { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "La descripción es requerida")]
    [MaxLength(10, ErrorMessage = "El nombre corto debe ser de máximo {1} caracteres")]
    public string ShortName { get; set; } = string.Empty;
}