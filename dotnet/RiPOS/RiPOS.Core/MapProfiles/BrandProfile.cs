using AutoMapper;
using RiPOS.Domain.Entities;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles;

public class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<Brand, BrandResponse>();

        CreateMap<BrandRequest, Brand>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()));
    }
}