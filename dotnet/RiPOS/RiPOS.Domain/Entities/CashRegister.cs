using RiPOS.Domain.Interfaces;
using RiPOS.Domain.Shared;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RiPOS.Domain.Entities
{
    public class CashRegister : TrackEntityChanges, IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public int StoreId { get; set; }

        [ForeignKey(nameof(StoreId))]
        public Store Store { get; set; }
    }
}
