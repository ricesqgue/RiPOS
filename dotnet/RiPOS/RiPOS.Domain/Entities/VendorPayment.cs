using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RiPOS.Domain.Interfaces;
using RiPOS.Domain.Shared;
using RiPOS.Shared.Enums;

namespace RiPOS.Domain.Entities;

public class VendorPayment : TrackEntityChanges, IEntity
{
    public int Id { get; set; }

    [Required]
    public int VendorDebtId { get; set; }
    [ForeignKey(nameof(VendorDebtId))]
    public VendorDebt? VendorDebt { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    [Required]
    [Column(TypeName = "timestamptz")]
    public DateTime PaymentDateTime { get; set; }

    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string? ReferenceCode { get; set; }
    
    [Column(TypeName = "text")]
    public string? Notes { get; set; }

    [Required]
    public int PaymentMethodId { get; set; }
    [ForeignKey(nameof(PaymentMethodId))] 
    public PaymentMethod? PaymentMethod { get; set; }
    
    [Required] 
    public bool IsDeleted { get; set; }
}