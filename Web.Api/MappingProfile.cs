using AutoMapper;
using Web.Application.DTOs;
using Web.Domain.Entities;

namespace Web.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserDtoView>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
        }

    }
}
