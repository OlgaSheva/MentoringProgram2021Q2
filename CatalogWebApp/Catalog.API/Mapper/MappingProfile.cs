using AutoMapper;
using Catalog.API.Models;
using Catalog.Services.Models;

namespace Catalog.API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<ProductDTO, Product>()
	            .ForMember(dest => dest.CategoryName,
	            opt => opt.MapFrom(src => src.Category.CategoryName))
	            .ForMember(dest => dest.SupplierName,
		            opt => opt.MapFrom(src => src.Supplier.CompanyName))
	            .ReverseMap();
            CreateMap<Product, ProductDTO>();
        }
    }
}
