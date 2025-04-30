using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RiPOS.Core.Interfaces;
using RiPOS.Domain.Entities;
using RiPOS.Repository.Interfaces;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.Services;

public class ProductService(IProductHeaderRepository productHeaderRepository, IProductDetailRepository productDetailRepository, IMapper mapper) : IProductService
{
    public async Task<ICollection<ProductHeaderResponse>> GetAllHeadersAsync(bool includeInactives = false)
    {
        var productHeaders = await productHeaderRepository
            .GetAllAsync(ph => ph.IsActive || includeInactives,
                  includeProps: ph => ph.Include(x => x.Brand!)
                        .Include(x => x.ProductCategories).ThenInclude(pc => pc.Category!)
                        .Include(x => x.ProductGenders).ThenInclude(pg => pg.Gender!));

        var productHeadersResponse = mapper.Map<ICollection<ProductHeaderResponse>>(productHeaders);
        return productHeadersResponse;
    }
    
    public async Task<ICollection<ProductHeaderWithDetailsResponse>> GetAllHeadersWithDetailsAsync(bool includeInactives = false)
    {
        var productHeaderWithDetails = await productHeaderRepository
            .GetAllAsync(ph => ph.IsActive || includeInactives,
                includeProps: ph =>
                    ph.Include(x => x.Brand!)
                        .Include(x => x.ProductCategories).ThenInclude(pc => pc.Category!)
                        .Include(x => x.ProductGenders).ThenInclude(pg => pg.Gender!)
                        .Include(x => x.ProductDetails.Where(pd => pd.IsActive || includeInactives)).ThenInclude(pd => pd.Size!)
                        .Include(x => x.ProductDetails.Where(pd => pd.IsActive || includeInactives))
                            .ThenInclude(pd => pd.ProductColors).ThenInclude(pc => pc.Color!));

        var productHeadersWithDetailsResponse = mapper.Map<ICollection<ProductHeaderWithDetailsResponse>>(productHeaderWithDetails);
        return productHeadersWithDetailsResponse;
    }
    
    public async Task<ICollection<ProductDetailResponse>> GetAllDetailsByHeaderIdAsync(int productHeaderId, bool includeInactives = false)
    {
        var productDetails = await productDetailRepository
            .GetAllAsync(pd => pd.ProductHeaderId == productHeaderId && (pd.IsActive || includeInactives),
                includeProps: pd => pd.Include(x => x.Size!)
                    .Include(x => x.ProductColors).ThenInclude(pc => pc.Color!));
        var productDetailsResponse = mapper.Map<ICollection<ProductDetailResponse>>(productDetails);
        return productDetailsResponse;
    }
    
    public async Task<bool> ExistsHeaderByIdAsync(int id)
    {
        return await productHeaderRepository.ExistsAsync(s => s.Id == id && s.IsActive);
    }
    
    public async Task<bool> ExistsDetailByIdAsync(int id)
    {
        return await productDetailRepository.ExistsAsync(s => s.Id == id && s.IsActive);
    }
    
    public async Task<MessageResponse<ProductHeaderResponse>> AddAsync(ProductHeaderRequest request, int userId)
    {
        var messageResponse = new MessageResponse<ProductHeaderResponse>();

        var productHeader = mapper.Map<ProductHeader>(request);

        productHeader.CreationByUserId = userId;
        productHeader.LastModificationByUserId = userId;
        productHeader.IsActive = true;
        
        var exists = await productHeaderRepository
            .FindAsync(c => (c.Name.ToUpper() == request.Name.Trim().ToUpper() ||  c.Sku.ToUpper() == request.Sku.Trim().ToUpper()) 
                            && c.IsActive);

        if (exists != null)
        {
            messageResponse.Success = false;
            if (string.Equals(exists.Name, request.Name.Trim(), StringComparison.CurrentCultureIgnoreCase))
            {
                messageResponse.Message = $"Ya existe un producto con el nombre \"{productHeader.Name}\"";
            }
            else
            {
                messageResponse.Message = $"Ya existe un producto con el SKU \"{productHeader.Sku}\"";
            }
            return messageResponse;
        }

        messageResponse.Success = await productHeaderRepository.AddAsync(productHeader);

        if (messageResponse.Success)
        {
            messageResponse.Success = true;
            messageResponse.Message = $"Producto agregado correctamente";
            messageResponse.Data = mapper.Map<ProductHeaderResponse>(productHeader);
        }
        else
        {
            messageResponse.Success = false;
            messageResponse.Message = "No se realizó ningún cambio";
        }

        return messageResponse;
    }
}