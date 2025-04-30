namespace RiPOS.Shared.Models.Responses;

public class ProductHeaderWithDetailsResponse : ProductHeaderResponse
{
    public ICollection<ProductDetailResponse> Details { get; set; } = new List<ProductDetailResponse>();
}