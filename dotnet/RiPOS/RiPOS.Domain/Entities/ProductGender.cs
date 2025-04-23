using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiPOS.Domain.Entities;

public class ProductGender
{
    [Required]
    public int ProductHeaderId { get; set; }
    [ForeignKey(nameof(ProductHeaderId))]
    public ProductHeader? ProductHeader { get; set; }
    
    [Required]
    public int GenderId { get; set; }
    [ForeignKey(nameof(GenderId))]
    public Gender? Gender { get; set; }
}