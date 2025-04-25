using RiPOS.Domain.Interfaces;
using RiPOS.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiPOS.Domain.Entities;

public class User : TrackEntityChanges, IEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public required string Name { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public required string Surname { get; set; }

    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string? SecondSurname { get; set; }

    [Required]
    [MaxLength(50)]
    [Column(TypeName = "varchar(50)")]
    public required string Username { get; set; }

    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string? Email { get; set; }

    [Required]
    [MaxLength(500)]
    [Column(TypeName = "varchar(500)")]
    public required string PasswordHash { get; set; }

    [MaxLength(25)]
    [Column(TypeName = "varchar(25)")]
    public string? PhoneNumber { get; set; }

    [MaxLength(25)]
    [Column(TypeName = "varchar(25)")]
    public string? MobilePhone { get; set; }

    [MaxLength(300)]
    [Column(TypeName = "varchar(300)")]
    public string? ProfileImagePath { get; set; }

    [Required] public bool IsActive { get; set; } = true;

    public ICollection<UserStoreRole>? UserStoreRoles { get; set; }
}