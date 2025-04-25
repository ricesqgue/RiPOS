using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Models.Requests;

public class StoreRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre de la tienda es requerido")]
    [MaxLength(50, ErrorMessage = "El nombre debe ser de máximo de {1} caracteres")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(400, ErrorMessage = "La dirección debe ser de máximo de {1} caracteres")]
    public string? Address { get; set; }

    [MaxLength(20, ErrorMessage = "El número de teléfono debe ser de máximo de {1} caracteres")]
    public string? PhoneNumber { get; set; }

    [MaxLength(20, ErrorMessage = "El número de celular debe ser de máximo de {1} caracteres")]
    public string? MobilePhone { get; set; }
}