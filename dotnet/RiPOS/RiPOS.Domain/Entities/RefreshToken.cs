using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiPOS.Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Token { get; set; }
    
    [Required]
    public DateTime Expires { get; set; }
    
    [Required]
    public bool IsRevoked { get; set; }

    [Required]
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}