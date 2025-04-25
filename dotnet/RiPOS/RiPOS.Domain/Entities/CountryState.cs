using RiPOS.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RiPOS.Domain.Entities;

public class CountryState : IEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public required string Name { get; set; }

    [Required]
    [MaxLength(10)]
    [Column(TypeName = "varchar(10)")]
    public required string ShortName { get; set; }
}