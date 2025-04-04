using RiPOS.Shared.Models.Responses;

namespace RiPOS.Repository.Repositories;

public class UserWithStoresResponse : UserResponse
{
    public ICollection<StoreResponse> Stores { get; set; }
}