using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Interfaces;

public interface IProductService
{
    Task<ICollection<ProductHeaderResponse>> GetAllHeadersAsync(bool includeInactives = false);
    Task<ICollection<ProductHeaderWithDetailsResponse>> GetAllHeadersWithDetailsAsync(bool includeInactives = false);
}