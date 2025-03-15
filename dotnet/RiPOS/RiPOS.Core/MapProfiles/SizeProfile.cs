using AutoMapper;
using RiPOS.Domain.Entities;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles
{
    public class SizeProfile : Profile
    {
        public SizeProfile()
        {
            CreateMap<Size, SizeResponse>();

            CreateMap<SizeRequest, Size>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
                .ForMember(dest => dest.ShortName, opt => opt.MapFrom(src => src.ShortName.ToUpper().Trim()));
        }
    }
}
