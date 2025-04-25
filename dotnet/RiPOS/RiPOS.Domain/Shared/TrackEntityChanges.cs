using RiPOS.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiPOS.Domain.Shared;

public class TrackEntityChanges
{
    [Column(TypeName = "timestamptz")]
    public DateTime CreationDateTime { get; set; }

    public int? CreationByUserId { get; set; }

    [ForeignKey(nameof(CreationByUserId))]
    public User? CreationByUser { get; set; }

    [Column(TypeName = "timestamptz")]
    public DateTime? LastModificationDateTime { get; set; }

    public int? LastModificationByUserId { get; set; }

    [ForeignKey(nameof(LastModificationByUserId))]
    public User? LastModificationByUser { get; set; }
}