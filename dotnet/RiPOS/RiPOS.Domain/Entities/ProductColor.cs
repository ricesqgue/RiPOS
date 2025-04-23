using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiPOS.Domain.Entities;

public class ProductColor
{
    public int ProductDetailsId { get; set; }
    [ForeignKey(nameof(ProductDetailsId))]
    public ProductDetails? ProductDetails { get; set; }
    
    [Required]
    public int ColorId { get; set; }
    [ForeignKey(nameof(ColorId))]
    public Color? Color { get; set; }
}