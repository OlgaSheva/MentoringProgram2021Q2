using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Catalog.Controllers;
using Catalog.Models;
using Catalog.Models.Northwind;
using CatalogTests.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CatalogTests.Controllers
{
    public class ProductsControllerTests
    {
        [Fact]
        public async void Index_OkResult()
        {
            var configuration = GetConfigurationRoot();
            var logger = new Mock<ILogger<ProductsController>>();
            var dbContext = new Mock<NorthwindContext>();
            dbContext.SetupGet(x => x.Products).Returns(TestFunctions.GetDbSet<Product>(TestData.Products).Object);

            var controller = new ProductsController(dbContext.Object, configuration, logger.Object);

            var result = await controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async void Create_OkResult()
        {
            var configuration = GetConfigurationRoot();
            var logger = new Mock<ILogger<ProductsController>>();
            var dbContext = new Mock<NorthwindContext>();
            dbContext.SetupGet(x => x.Products).Returns(TestFunctions.GetDbSet<Product>(TestData.Products).Object);

            var controller = new ProductsController(dbContext.Object, configuration, logger.Object);
            await controller.Create(new ProductEditViewModel() { Product = new Product() { ProductId = 3, ProductName = "testName3" } });

            dbContext.Verify(c => c.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
            dbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Creating product id = 3", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async void Edit_OkResult()
        {
            var configuration = GetConfigurationRoot();
            var logger = new Mock<ILogger<ProductsController>>();
            var dbContext = new Mock<NorthwindContext>();
            dbContext.SetupGet(x => x.Products).Returns(TestFunctions.GetDbSet<Product>(TestData.Products).Object);

            var controller = new ProductsController(dbContext.Object, configuration, logger.Object);
            await controller.Edit(1, new ProductEditViewModel() { Product = new Product() { ProductId = 1, ProductName = "testName3" } });

            dbContext.Verify(c => c.Update(It.IsAny<Product>()), Times.Once);
            dbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Editing product id = 1", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async void Delete_OkResult()
        {
            var configuration = GetConfigurationRoot();
            var logger = new Mock<ILogger<ProductsController>>();
            var dbContext = new Mock<NorthwindContext>();
            dbContext.SetupGet(x => x.Products).Returns(TestFunctions.GetDbSet<Product>(TestData.Products).Object);

            var controller = new ProductsController(dbContext.Object, configuration, logger.Object);
            await controller.Delete(1);

            dbContext.Verify(c => c.Remove(It.IsAny<Product>()), Times.Once);
            dbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Removing product id = 1", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
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
    }
}
