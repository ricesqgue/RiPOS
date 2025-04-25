namespace RiPOS.Shared.Models.Responses;

public class StoreResponse
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? MobilePhone { get; set; }

    public bool IsActive { get; set; }
}