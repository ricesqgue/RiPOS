using AutoMapper;
using RiPOS.Domain.Entities;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyResponse>();
        }
    }
}
