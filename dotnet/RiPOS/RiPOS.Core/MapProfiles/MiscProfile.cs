using AutoMapper;
using RiPOS.Domain.Entities;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles
{
    public class MiscProfile : Profile
    {
        public MiscProfile()
        {
            CreateMap<CountryState, CountryStateResponse>();
        }
    }
}
