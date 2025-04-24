using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RiPOS.Domain.Interfaces;
using RiPOS.Domain.Shared;

namespace RiPOS.Domain.Entities;

public class PurchaseOrderNote : TrackEntityChanges, IEntity
{
    public int Id { get; set; }

    [Required]
    public int PurchaseOrderId { get; set; }
    [ForeignKey(nameof(PurchaseOrderId))]
    public PurchaseOrder? PurchaseOrder { get; set; }
    
    [Required]
    [Column(TypeName = "text")]
    public required string Note { get; set; }

    [Required]
    public bool IsDeleted { get; set; } = false;
    
}