using System.ComponentModel.DataAnnotations;

namespace RiPOS.Shared.Models.Requests
{
    public class GenderRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del género es requerido")]
        [MaxLength(50, ErrorMessage = "El nombre debe ser de máximo de {1} caracteres")]
        public string Name { get; set; }
    }
}
