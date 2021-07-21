using AutoMapper;
using Catalog.Models.ViewModels;
using Catalog.Services.Models;
using Catalog.ViewModels;

namespace Catalog.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDTO, CategoryViewModel>();
            CreateMap<CategoryCreateViewModel, CategoryDTO>();
            CreateMap<ProductDTO, ProductViewModel>();
            CreateMap<ProductViewModel, ProductDTO>();
        }
    }
}
