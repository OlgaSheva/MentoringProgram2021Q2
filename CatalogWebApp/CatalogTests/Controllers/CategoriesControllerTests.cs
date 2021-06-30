using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.Controllers;
using Catalog.Models;
using Catalog.Models.Northwind;
using CatalogTests.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Times = Moq.Times;

namespace CatalogTests.Controllers
{
    public class CategoriesControllerTests
    {
        [Fact]
        public async void Index_OkResult()
        {
            var logger = new Mock<ILogger<CategoriesController>>();
            var dbContext = new Mock<NorthwindContext>();
            dbContext.SetupGet(x => x.Categories).Returns(TestFunctions.GetDbSet<Category>(TestData.Categories).Object);

            var controller = new CategoriesController(dbContext.Object, logger.Object);
            var result = await controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<Category>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Create_OkResult()
        {
            var logger = new Mock<ILogger<CategoriesController>>();
            var dbContext = new Mock<NorthwindContext>();
            dbContext.SetupGet(x => x.Categories).Returns(TestFunctions.GetDbSet<Category>(TestData.Categories).Object);

            var controller = new CategoriesController(dbContext.Object, logger.Object);
            await controller.Create(new CategoryViewModel() { CategoryName = "category3" });

            dbContext.Verify(c => c.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
            dbContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
