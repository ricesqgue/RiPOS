using AutoMapper;
using RiPOS.Domain.Entities;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerResponse>();

            CreateMap<CustomerRequest, Customer>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname.Trim()))
                .ForMember(dest => dest.SecondSurname, opt => opt.MapFrom(src => src.SecondSurname.Trim()))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber.Trim()))
                .ForMember(dest => dest.MobilePhone, opt => opt.MapFrom(src => src.MobilePhone.Trim()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()))
                .ForMember(dest => dest.Rfc, opt => opt.MapFrom(src => src.Rfc.Trim().ToUpper()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address.Trim()))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Trim()))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.ZipCode.Trim()));
        }
    }
}
