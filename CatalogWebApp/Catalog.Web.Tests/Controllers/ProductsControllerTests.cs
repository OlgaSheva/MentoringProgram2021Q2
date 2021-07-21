using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Catalog.Controllers;
using Catalog.Models.ViewModels;
using Catalog.Services.Interfaces;
using Catalog.Services.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit;

namespace CatalogTests.Controllers
{
    public class ProductsControllerTests
    {
        private IEnumerable<ProductDTO> _productDtos = new ProductDTO[] { new() { ProductId = 1 }, new() { ProductId = 2 } };
        private IEnumerable<ProductViewModel> _productViewModels = new ProductViewModel[] { new() { ProductId = 1 }, new() { ProductId = 2 } };

        [Fact]
        public async void Index_OkResult()
        {
            var fakes = GetFakes();
            fakes.configuration = GetConfigurationRoot();
            A.CallTo(() => fakes.productService.GetProductsAsync(A<int>._)).Returns(_productDtos);
            A.CallTo(() => fakes.mapper.Map<IEnumerable<ProductViewModel>>(A<IEnumerable<ProductDTO>>._)).Returns(_productViewModels);

            var controller = new ProductsController(fakes.configuration, fakes.logger, fakes.productService, fakes.categoryService, fakes.supplierService, fakes.mapper);

            var result = await controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<ProductViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
            A.CallTo(() => fakes.productService.GetProductsAsync(A<int>._)).MustHaveHappened();
            A.CallTo(() => fakes.mapper.Map<IEnumerable<ProductViewModel>>(A<IEnumerable<ProductDTO>>._)).MustHaveHappened();
        }

        [Fact]
        public async void Create_OkResult()
        {
            var fakes = GetFakes();
            fakes.configuration = GetConfigurationRoot();

            var controller = new ProductsController(fakes.configuration, fakes.logger, fakes.productService, fakes.categoryService, fakes.supplierService, fakes.mapper);
            await controller.Create(new ProductEditViewModel() { Product = new ProductDTO() { ProductId = 3, ProductName = "testName3" } });

            A.CallTo(() => fakes.productService.AddAsync(A<ProductDTO>._)).MustHaveHappened();
        }

        [Fact]
        public async void Edit_OkResult()
        {
            var fakes = GetFakes();
            fakes.configuration = GetConfigurationRoot();

            var controller = new ProductsController(fakes.configuration, fakes.logger, fakes.productService, fakes.categoryService, fakes.supplierService, fakes.mapper);
            await controller.Edit(1, new ProductEditViewModel() { Product = new ProductDTO() { ProductId = 1, ProductName = "testName3" } });

            A.CallTo(() => fakes.productService.EditAsync(A<ProductDTO>._)).MustHaveHappened();
        }

        private static IConfigurationRoot GetConfigurationRoot()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"ProductSettings:MaxShownProductCount", "3"},
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            return configuration;
        }

        private (IConfiguration configuration,
            ILogger<ProductsController> logger,
            IProductService productService,
            ICategoryService categoryService,
            ISupplierService supplierService,
            IMapper mapper) GetFakes()
        {
            var configuration = A.Fake<IConfiguration>();
            var logger = A.Fake<ILogger<ProductsController>>();
            var productService = A.Fake<IProductService>();
            var categoryService = A.Fake<ICategoryService>();
            var supplierService = A.Fake<ISupplierService>();
            var mapper = A.Fake<IMapper>();
            return (configuration, logger, productService, categoryService, supplierService, mapper);
        }
    }
}
