namespace RiPOS.Shared.Models.Responses;

public class GenderResponse
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public bool IsActive { get; set; }
}