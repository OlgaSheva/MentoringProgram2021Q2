using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Controllers;
using Catalog.Models.ViewModels;
using Catalog.Services.Interfaces;
using Catalog.Services.Models;
using Catalog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using FakeItEasy;

namespace CatalogTests.Controllers
{
    public class CategoriesControllerTests
    {
        private IEnumerable<CategoryDTO> categoryDtos = new CategoryDTO[] { new() { CategoryId = 1 }, new() { CategoryId = 2 } };
        private IEnumerable<CategoryViewModel> categoryViewModels = new CategoryViewModel[] { new() { CategoryId = 1 }, new() { CategoryId = 2 } };

        [Fact]
        public async void Index_OkResult()
        {
            var logger = A.Fake<ILogger<CategoriesController>>();
            var service = A.Fake<ICategoryService>();
            var mapper = A.Fake<IMapper>();
            A.CallTo(() => service.GetAllAsync()).Returns(categoryDtos);
            A.CallTo(() => mapper.Map<IEnumerable<CategoryViewModel>>(A<IEnumerable<CategoryDTO>>._)).Returns(categoryViewModels);
            
            var controller = new CategoriesController(logger, service, mapper);
            var result = await controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<CategoryViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
            A.CallTo(() => service.GetAllAsync()).MustHaveHappened();
            A.CallTo(() => mapper.Map<IEnumerable<CategoryViewModel>>(A<IEnumerable<CategoryDTO>>._)).MustHaveHappened();
        }

        [Fact]
        public async Task Create_OkResult()
        {
            var logger = A.Fake<ILogger<CategoriesController>>();
            var service = A.Fake<ICategoryService>();
            var mapper = A.Fake<IMapper>();

            var controller = new CategoriesController(logger, service, mapper);
            await controller.Create(new CategoryCreateViewModel() { CategoryName = "category3" });
            
            A.CallTo(() => service.AddAsync(A<CategoryDTO>._)).MustHaveHappened();
        }
    }
}
