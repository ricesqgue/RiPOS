using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiPOS.Domain.Entities;

public class InventoryTransferDetail
{
    public int Id { get; set; }
    
    [Required]
    public required int Quantity { get; set; }

    [Required]
    public int ProductDetailId { get; set; }
    [ForeignKey(nameof(ProductDetailId))]
    public ProductDetail? ProductDetail { get; set; }

    [Required]
    public int InventoryTransferId { get; set; }
    [ForeignKey(nameof(InventoryTransferId))]
    public InventoryTransfer? InventoryTransfer { get; set; }
}