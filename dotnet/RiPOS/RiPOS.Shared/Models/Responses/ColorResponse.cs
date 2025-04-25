namespace RiPOS.Shared.Models.Responses;

public class ColorResponse
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string RgbHex { get; set; }

    public bool IsActive { get; set; }
}