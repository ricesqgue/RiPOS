using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiPOS.Domain.Entities
{
    public class UserStoreRole
    {
        [Required]
        public int UserId { get; set; }
        
        [ForeignKey(nameof(UserId))]
        public required User User { get; set; }

        [Required]
        public int StoreId { get; set; }
        
        [ForeignKey(nameof(StoreId))]
        public required Store Store { get; set; }

        [Required]
        public int RoleId { get; set; }
        
        [ForeignKey(nameof(RoleId))]
        public required Role Role { get; set; }
    }
}
