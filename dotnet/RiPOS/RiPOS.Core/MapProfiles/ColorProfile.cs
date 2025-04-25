using AutoMapper;
using RiPOS.Domain.Entities;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles;

public class ColorProfile : Profile
{
    public ColorProfile()
    {
        CreateMap<Color, ColorResponse>();

        CreateMap<ColorRequest, Color>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
            .ForMember(dest => dest.RgbHex, opt => opt.MapFrom(src => src.RgbHex.Trim()));
    }
}