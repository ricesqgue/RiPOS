using RiPOS.Shared.Models.Responses;

namespace RiPOS.Shared.Models.Requests;

public class ProductHeaderWithDetailsRequest : ProductHeaderRequest
{
    public ICollection<ProductDetailRequest> Details { get; set; } = new List<ProductDetailRequest>();
}