using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Models.Requests;

public class CashRegisterRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre de la caja es requerido")]
    [MaxLength(50, ErrorMessage = "El nombre debe ser de máximo de {1} caracteres")]
    public string Name { get; set; } = string.Empty;
}