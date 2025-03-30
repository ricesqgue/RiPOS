using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RiPOS.Domain.Interfaces;

namespace RiPOS.Domain.Entities;

public class RefreshToken : IEntity
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string Token { get; set; }
    
    [Required]
    public DateTime Expires { get; set; }
    
    [Required]
    public DateTime Created { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public required User User { get; set; }
}