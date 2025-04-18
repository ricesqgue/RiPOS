namespace RiPOS.Shared.Models.Responses;

public class UserWithStoresResponse : UserResponse
{
    public required ICollection<StoreResponse> Stores { get; set; }
}