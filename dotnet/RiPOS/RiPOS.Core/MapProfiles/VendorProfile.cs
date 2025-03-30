using AutoMapper;
using RiPOS.Domain.Entities;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles
{
    public class VendorProfile : Profile
    {
        public VendorProfile()
        {
            CreateMap<Vendor, VendorResponse>();

            CreateMap<VendorRequest, Vendor>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname.Trim()))
                .ForMember(dest => dest.SecondSurname, opt => opt.MapFrom(src => src.SecondSurname != null ? src.SecondSurname.Trim() : null))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber != null ? src.PhoneNumber.Trim() : null))
                .ForMember(dest => dest.MobilePhone, opt => opt.MapFrom(src => src.MobilePhone != null ? src.MobilePhone.Trim() : null))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email != null ? src.Email.Trim() : null))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address != null ? src.Address.Trim() : null))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City != null ? src.City.Trim() : null))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.ZipCode != null ?  src.ZipCode.Trim() : null));
        }
    }
}
