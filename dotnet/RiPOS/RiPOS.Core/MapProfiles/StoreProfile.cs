using AutoMapper;
using RiPOS.Domain.Entities;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles
{
    public class StoreProfile : Profile
    {
        public StoreProfile()
        {
            CreateMap<Store, StoreResponse>();

            CreateMap<StoreRequest, Store>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address.Trim()))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.Trim()))
                .ForMember(dest => dest.MobilePhone, opt => opt.MapFrom(src => src.MobilePhone.Trim()));
        }
    }
}
