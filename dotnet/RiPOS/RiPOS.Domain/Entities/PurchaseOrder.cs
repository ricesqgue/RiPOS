using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RiPOS.Domain.Interfaces;
using RiPOS.Domain.Shared;

namespace RiPOS.Domain.Entities;

public class PurchaseOrder : TrackEntityChanges, IEntity
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public required string InvoiceNumber { get; set; }

    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal TotalAmount { get; set; }
    
    [Required]
    [Column(TypeName = "timestamptz")]
    public DateTime OrderDateTime { get; set; }

    [Required]
    public int PurchaseOrderStatusId { get; set; }
    [ForeignKey(nameof(PurchaseOrderStatusId))]
    public PurchaseOrderStatus? PurchaseOrderStatus { get; set; }
    
    [Required]
    public int VendorId { get; set; }
    [ForeignKey(nameof(VendorId))]
    public Vendor? Vendor { get; set; }
    
    [Required]
    public int StoreId { get; set; }
    [ForeignKey(nameof(StoreId))]
    public Store? Store { get; set; }
    
    public ICollection<PurchaseOrderDetail> Details { get; set; } = new List<PurchaseOrderDetail>();
    
    public ICollection<PurchaseOrderNote> Notes { get; set; } = new List<PurchaseOrderNote>();
}