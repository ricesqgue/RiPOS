using AutoMapper;
using RiPOS.Domain.Entities;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductHeader, ProductHeaderResponse>();

        CreateMap<ProductHeaderRequest, ProductHeader>()
            .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku.Trim()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
            .ForMember(dest => dest.Description,
                opt => opt.MapFrom(src => src.Description != null ? src.Description.Trim() : null));
        
        CreateMap<ProductDetail, ProductDetailResponse>();
        
        CreateMap<ProductDetailRequest, ProductDetail>()
            .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(src => src.ProductCode.Trim()))
            .ForMember(dest => dest.VariantName,
                opt => opt.MapFrom(src => src.VariantName != null ? src.VariantName.Trim() : null));
    }
}