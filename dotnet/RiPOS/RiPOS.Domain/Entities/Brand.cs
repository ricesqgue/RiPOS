using RiPOS.Domain.Interfaces;
using RiPOS.Domain.Shared;
using System.ComponentModel.DataAnnotations;

namespace RiPOS.Domain.Entities
{
    public class Brand : TrackEntityChanges, IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(300)]
        public string? LogoPath { get; set; }

        [Required] 
        public bool IsActive { get; set; } = true;
    }
}
