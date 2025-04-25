using RiPOS.Domain.Interfaces;
using RiPOS.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiPOS.Domain.Entities;

public class Store : TrackEntityChanges, IEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public required string Name { get; set; }

    [MaxLength(400)]
    [Column(TypeName = "varchar(400)")]
    public string? Address { get; set; }

    [MaxLength(25)]
    [Column(TypeName = "varchar(25)")]
    public string? PhoneNumber { get; set; }

    [MaxLength(25)]
    [Column(TypeName = "varchar(25)")]
    public string? MobilePhone { get; set; }

    [MaxLength(300)]
    [Column(TypeName = "varchar(300)")]
    public string? LogoPath { get; set; }

    [Required] 
    public bool IsActive { get; set; } = true;
}