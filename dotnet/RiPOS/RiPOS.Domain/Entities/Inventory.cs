using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiPOS.Domain.Entities;

public class Inventory
{
    [Required]
    public int StoreId { get; set; }
    [ForeignKey(nameof(StoreId))]
    public Store? Store { get; set; }
    
    [Required]
    public int ProductDetailId { get; set; }
    [ForeignKey(nameof(ProductDetailId))]
    public ProductDetail? ProductDetail { get; set; }
    
    [Required] 
    public int Quantity { get; set; } = 0;
}