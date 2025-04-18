using RiPOS.Domain.Interfaces;
using RiPOS.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiPOS.Domain.Entities
{
    public class Vendor : TrackEntityChanges, IEntity
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

        [MaxLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string? PhoneNumber { get; set; }

        [MaxLength(20)]
        [Column(TypeName = "varchar(20)")]
        public string? MobilePhone { get; set; }

        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string? Email { get; set; }

        [MaxLength(400)]
        [Column(TypeName = "varchar(400)")]
        public string? Address { get; set; }

        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string? City { get; set; }

        [MaxLength(10)]
        [Column(TypeName = "varchar(10)")]
        public string? ZipCode { get; set; }

        [Required]
        public int CountryStateId { get; set; }

        [ForeignKey(nameof(CountryStateId))]
        public CountryState? CountryState { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}