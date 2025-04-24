using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RiPOS.Domain.Interfaces;
using RiPOS.Domain.Shared;

namespace RiPOS.Domain.Entities;

public class VendorDebt : TrackEntityChanges, IEntity
{
    public int Id { get; set; }

    [Required]
    public int PurchaseOrderId { get; set; }
    [ForeignKey(nameof(PurchaseOrderId))]
    public PurchaseOrder? PurchaseOrder { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal RemainingAmount { get; set; }
    
    [Required]
    [Column(TypeName = "timestamptz")]
    public DateTime DueDate { get; set; }
}