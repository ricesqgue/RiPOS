using AutoMapper;
using RiPOS.Domain.Entities;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles;

public class StoreProfile : Profile
{
    public StoreProfile()
    {
        CreateMap<Store, StoreResponse>();

        CreateMap<StoreRequest, Store>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address != null ? src.Address.Trim() : null))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber != null ? src.PhoneNumber.Trim() : null))
            .ForMember(dest => dest.MobilePhone, opt => opt.MapFrom(src => src.MobilePhone != null ? src.MobilePhone.Trim() : null));
    }
}