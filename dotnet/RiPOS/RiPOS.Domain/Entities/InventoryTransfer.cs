using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RiPOS.Domain.Interfaces;

namespace RiPOS.Domain.Entities;

public class InventoryTransfer : IEntity
{
    public int Id { get; set; }
    
    [Required]
    public int FromStoreId { get; set; }
    [ForeignKey(nameof(FromStoreId))]
    public Store? FromStore { get; set; }
    
    [Required]
    public int ToStoreId { get; set; }
    [ForeignKey(nameof(ToStoreId))]
    public Store? ToStore { get; set; }

    [Required]
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    [Column(TypeName = "timestamptz")]
    public DateTime TransferDateTime { get; set; } = DateTime.UtcNow;
}