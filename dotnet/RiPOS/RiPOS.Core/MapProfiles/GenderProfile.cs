using AutoMapper;
using RiPOS.Domain.Entities;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles;

public class GenderProfile : Profile
{
    public GenderProfile()
    {
        CreateMap<Gender, GenderResponse>();

        CreateMap<GenderRequest, Gender>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()));
    }
}