using AutoMapper;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class MappingProfile : Profile {
    public MappingProfile() {
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
        CreateMap<Product, ReadProductDto>();

        CreateMap<Category, ReadCategoryDto>();

        CreateMap<RegisterUserDto, User>();
    }
}