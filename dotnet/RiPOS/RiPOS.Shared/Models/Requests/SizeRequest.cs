using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Models.Requests
{
    public class SizeRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre de la talla es requerido")]
        [MaxLength(50, ErrorMessage = "El nombre debe ser de máximo de {1} caracteres")]
        public required string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "La descripción es requerida")]
        [MaxLength(10, ErrorMessage = "El nombre corto debe ser de máximo {1} caracteres")]
        public required string ShortName { get; set; }
    }
}
