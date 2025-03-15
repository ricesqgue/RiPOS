using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Models.Requests
{
    public class BrandRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre de la marca es requerido")]
        [MaxLength(50, ErrorMessage = "El nombre debe ser de máximo de {1} caracteres")]
        public string Name { get; set; }
    }
}
