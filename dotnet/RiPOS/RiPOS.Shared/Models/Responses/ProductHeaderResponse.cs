namespace RiPOS.Shared.Models.Responses;

public class ProductHeaderResponse
{
    public int Id { get; set; }

    public required string Sku { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public decimal BasePrice { get; set; }

    public int BrandId { get; set; }
    
    public BrandResponse? Brand { get; set; }

    public ICollection<CategoryResponse> Categories { get; set; } = new List<CategoryResponse>();

    public ICollection<GenderResponse> Genders { get; set; } = new List<GenderResponse>();
    
    public bool IsActive { get; set; }
}