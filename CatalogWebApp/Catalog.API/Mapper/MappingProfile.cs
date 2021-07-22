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
            CreateMap<ProductDTO, Product>();
            CreateMap<Product, ProductDTO>();
            CreateMap<SupplierDTO, Supplier>();
            CreateMap<Supplier, SupplierDTO>();
        }
    }
}
