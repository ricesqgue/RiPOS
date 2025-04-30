using System.ComponentModel.DataAnnotations;
using RiPOS.Shared.Utilities.ValidationAttributes;

namespace RiPOS.Shared.Models.Requests;

public class ProductHeaderRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "El SKU es requerido")]
    [MaxLength(50, ErrorMessage = "El SKU debe ser de máximo de {1} caracteres")]
    public string Sku { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es requerido")]
    [MaxLength(100, ErrorMessage = "El nombre debe ser de máximo de {1} caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(100, ErrorMessage = "La descripción debe ser de máximo de {1} caracteres")]
    public string? Description { get; set; }
    
    [DecimalNumber(AllowNegative = false, AllowZero = true)]
    [Display(Name = "Precio base")]
    public decimal BasePrice { get; set; }
    
    [IntegerGreaterThanZero(ErrorMessage = "La marca es requerida")]
    public int BrandId { get; set; }

    [IntCollectionNotEmpty(AllowZero = false)]
    public ICollection<int> CategoryIds { get; set; } = new List<int>();

    [IntCollectionNotEmpty(AllowZero = false)]
    public ICollection<int> GenderIds { get; set; } = new List<int>();
}