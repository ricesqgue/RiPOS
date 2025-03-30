using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Models.Requests
{
    public class ColorRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del color es requerido")]
        [MaxLength(50, ErrorMessage = "El nombre debe ser de máximo de {1} caracteres")]
        public required string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El valor del color RGB es requerido")]
        [MaxLength(10, ErrorMessage = "El valor del color RGB debe ser de máximo {1} caracteres")]
        public required string RgbHex { get; set; }
    }
}
