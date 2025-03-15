using AutoMapper;
using RiPOS.Domain.Entities;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles
{
    public class CashRegisterProfile : Profile
    {
        public CashRegisterProfile()
        {
            CreateMap<CashRegister, CashRegisterResponse>();

            CreateMap<CashRegisterRequest, CashRegister>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()));
        }
    }
}
