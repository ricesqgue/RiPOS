using System.ComponentModel;
using RiPOS.Domain.Interfaces;
using RiPOS.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiPOS.Domain.Entities
{
    public class User : TrackEntityChanges, IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Surname { get; set; }

        [MaxLength(50)]
        public string? SecondSurname { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Username { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(500)]
        public required string PasswordHash { get; set; }

        [MaxLength(25)]
        public string? PhoneNumber { get; set; }

        [MaxLength(25)]
        public string? MobilePhone { get; set; }

        [MaxLength(300)]
        public string? ProfileImagePath { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public required ICollection<UserStoreRole> UserStoreRoles { get; set; }
    }
}
