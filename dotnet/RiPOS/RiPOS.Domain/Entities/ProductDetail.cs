using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RiPOS.Domain.Entities;
using RiPOS.Domain.Interfaces;
using RiPOS.Domain.Shared;

namespace RiPOS.Domain.Entities;

public class ProductDetail : TrackEntityChanges, IEntity
{
    public int Id { get; set; }
    
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string? VariantName { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(100)")]
    public required string ProductCode { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal AdditionalPrice { get; set; } = 0;
    
    [Required]
    public int ProductHeaderId { get; set; }
    [ForeignKey(nameof(ProductHeaderId))]
    public ProductHeader? ProductHeader { get; set; }
    
    [Required]
    public int SizeId { get; set; }
    [ForeignKey(nameof(SizeId))]
    public Size? Size { get; set; }
    
    [Required] 
    public required bool IsActive { get; set; } = true;

    public ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
}