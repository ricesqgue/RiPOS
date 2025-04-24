using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RiPOS.Domain.Interfaces;
using RiPOS.Domain.Shared;

namespace RiPOS.Domain.Entities;

public class PurchaseOrderDetail : TrackEntityChanges, IEntity
{
    public int Id { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal UnitCost { get; set; }

    [Required]
    public int Quantity { get; set; }
    
    [Required]
    public int PurchaseOrderId { get; set; }
    [ForeignKey(nameof(PurchaseOrderId))]
    public PurchaseOrder? PurchaseOrder { get; set; }
    
    [Required]
    public int ProductDetailId { get; set; }
    [ForeignKey(nameof(ProductDetailId))]
    public ProductDetail? ProductDetail { get; set; }

    [NotMapped]
    public decimal SubTotal => UnitCost * Quantity;
}