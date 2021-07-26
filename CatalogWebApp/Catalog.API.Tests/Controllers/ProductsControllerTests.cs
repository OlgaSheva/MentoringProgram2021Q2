using System.Collections.Generic;
using AutoMapper;
using Catalog.API.Controllers;
using Catalog.API.Models;
using Catalog.Services.Interfaces;
using Catalog.Services.Models;
using Microsoft.Extensions.Logging;
using Xunit;
using FakeItEasy;

namespace Catalog.API.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private IEnumerable<ProductDTO> _productDtos = new ProductDTO[] { new() { ProductId = 1 }, new() { ProductId = 2 } };
        private IEnumerable<Product> _Products = new Product[] { new() { ProductId = 1 }, new() { ProductId = 2 } };

        [Fact]
        public async void Index_OkResult()
        {
            var fakes = GetFakes();
            A.CallTo(() => fakes.productService.GetProductsAsync(A<int>._)).Returns(_productDtos);
            A.CallTo(() => fakes.mapper.Map<IEnumerable<Product>>(A<IEnumerable<ProductDTO>>._)).Returns(_Products);

            var controller = new ProductsController(fakes.logger, fakes.productService, fakes.categoryService, fakes.supplierService, fakes.mapper);

            var result = await controller.Get();

            A.CallTo(() => fakes.productService.GetProductsAsync(A<int>._)).MustHaveHappened();
            A.CallTo(() => fakes.mapper.Map<IEnumerable<Product>>(A<IEnumerable<ProductDTO>>._)).MustHaveHappened();
        }

        [Fact]
        public async void Create_OkResult()
        {
            var fakes = GetFakes();

            var controller = new ProductsController(fakes.logger, fakes.productService, fakes.categoryService, fakes.supplierService, fakes.mapper);
            await controller.Post(3, new Product() { ProductId = 3, ProductName = "testName3" });

            A.CallTo(() => fakes.productService.AddAsync(A<ProductDTO>._)).MustHaveHappened();
        }

        [Fact]
        public async void Edit_OkResult()
        {
            var fakes = GetFakes();

            var controller = new ProductsController(fakes.logger, fakes.productService, fakes.categoryService, fakes.supplierService, fakes.mapper);
            await controller.Put(1, new Product() { ProductId = 1, ProductName = "testName3" });

            A.CallTo(() => fakes.productService.EditAsync(A<ProductDTO>._)).MustHaveHappened();
        }

        private (
            ILogger<ProductsController> logger,
            IProductService productService,
            ICategoryService categoryService,
            ISupplierService supplierService,
            IMapper mapper) GetFakes()
        {
            var logger = A.Fake<ILogger<ProductsController>>();
            var productService = A.Fake<IProductService>();
            var categoryService = A.Fake<ICategoryService>();
            var supplierService = A.Fake<ISupplierService>();
            var mapper = A.Fake<IMapper>();
            return (logger, productService, categoryService, supplierService, mapper);
        }
    }
}
