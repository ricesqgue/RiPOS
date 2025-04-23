using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RiPOS.Domain.Interfaces;
using RiPOS.Domain.Shared;

namespace RiPOS.Domain.Entities;

public class ProductHeader : TrackEntityChanges, IEntity
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public required string Sku { get; set; }

    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public required string Name { get; set; }
    
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string? Description { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal BasePrice { get; set; }
    
    [Required]
    public int BrandId { get; set; }
    [ForeignKey(nameof(BrandId))]
    public Brand? Brand { get; set; }

    [Required] 
    public required bool IsActive { get; set; } = true;
    
    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public ICollection<ProductGender> ProductGenders { get; set; } = new List<ProductGender>();

    public ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}