using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Models.Requests
{
    public class CategoryRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre de la categoría es requerido")]
        [MaxLength(50, ErrorMessage = "El nombre debe ser de máximo de {1} caracteres")]
        public required string Name { get; set; }
    }
}
