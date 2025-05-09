using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiPOS.Domain.Entities;

public class ProductColor
{
    public int ProductDetailId { get; set; }
    [ForeignKey(nameof(ProductDetailId))]
    public ProductDetail? ProductDetail { get; set; }
    
    [Required]
    public int ColorId { get; set; }
    [ForeignKey(nameof(ColorId))]
    public Color? Color { get; set; }
}