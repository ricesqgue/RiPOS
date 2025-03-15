using AutoMapper;
using RiPOS.Domain.Entities;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles
{
    public class MiscellaneousProfile : Profile
    {
        public MiscellaneousProfile()
        {
            CreateMap<CountryState, CountryStateResponse>();
        }
    }
}
