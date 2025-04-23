using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiPOS.Domain.Entities;

public class ProductCategory
{
    [Required]
    public int ProductHeaderId { get; set; }
    [ForeignKey(nameof(ProductHeaderId))]
    public ProductHeader? ProductHeader { get; set; }
    
    [Required]
    public int CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }
}