using System.ComponentModel.DataAnnotations;
using RiPOS.Shared.Utilities.ValidationAttributes;

namespace RiPOS.Shared.Models.Requests;

public class ProductDetailRequest
{
    [MaxLength(100, ErrorMessage = "La variante debe ser de máximo de {1} caracteres")]
    public string? VariantName { get; set; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "El código de producto es requerido")]
    [MaxLength(100, ErrorMessage = "El código de producto debe ser de máximo de {1} caracteres")]
    public required string ProductCode { get; set; }
    
    [DecimalNumber(AllowNegative = true, AllowZero = true)]
    [Display(Name = "Precio adicional")]
    public decimal AdditionalPrice { get; set; }
    
    [IntegerGreaterThanZero(ErrorMessage = "La talla es requerida")]
    public int SizeId { get; set; }

    [IntCollectionNotEmpty(AllowZero = false)]
    public ICollection<int> ColorIds { get; set; } = new List<int>();
}