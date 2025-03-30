using RiPOS.Shared.Enums;

namespace RiPOS.Shared.Models.Session;

public class UserSession
{
    public required int UserId { get; set; }

    public required int StoreId { get; set; }
}