using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiPOS.Domain.Entities
{
    public class UserStoreRole
    {
        [Required]
        public int UserId { get; set; }
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Required]
        public int StoreId { get; set; }
        
        [ForeignKey(nameof(StoreId))]
        public Store Store { get; set; }

        [Required]
        public int RoleId { get; set; }
        
        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }
    }
}
