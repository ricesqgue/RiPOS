using AutoMapper;
using RiPOS.Domain.Entities;
using RiPOS.Shared.Models.Requests;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.Core.MapProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryResponse>();

        CreateMap<CategoryRequest, Category>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()));
    }
}