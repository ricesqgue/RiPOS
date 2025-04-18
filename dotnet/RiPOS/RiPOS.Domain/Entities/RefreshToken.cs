using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RiPOS.Domain.Interfaces;

namespace RiPOS.Domain.Entities;

public class RefreshToken : IEntity
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public required string Token { get; set; }
    
    [Required]
    [Column(TypeName = "timestamptz")]
    public DateTime Expires { get; set; }
    
    [Required]
    [Column(TypeName = "timestamptz")]
    public DateTime Created { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}