namespace RiPOS.Shared.Models.Responses;

public class ProductDetailResponse
{
    public int Id { get; set; }

    public string? VariantName { get; set; }

    public required string ProductCode { get; set; }

    public decimal AdditionalPrice { get; set; }

    public int SizeId { get; set; }

    public SizeResponse? Size { get; set; }

    public ICollection<ColorResponse> Colors { get; set; } = new List<ColorResponse>();

    public bool IsActive { get; set; }
}