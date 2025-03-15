using RiPOS.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RiPOS.Domain.Entities
{
    public class Company : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Code { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime CreationDate { get; set; }
    }
}
