namespace RiPOS.Shared.Models.Responses;

public class LoginResponse
{
    public required string AccessToken { get; set; }
    public DateTime Expires { get; set; }
    
    public required ICollection<StoreResponse> AvailableStores { get; set; }
}