using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RiPOS.Domain.Interfaces;

namespace RiPOS.Domain.Entities;

public class PurchaseOrderStatus : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public required string Name { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;
}