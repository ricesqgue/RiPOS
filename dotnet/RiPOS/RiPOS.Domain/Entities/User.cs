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
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        [MaxLength(50)]
        public string SecondSurname { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [MaxLength(25)]
        public string PhoneNumber { get; set; }

        [MaxLength(25)]
        public string MobilePhone { get; set; }

        [MaxLength(300)]
        public string ProfileImagePath { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public ICollection<UserStoreRole> UserStoreRoles { get; set; }
    }
}
