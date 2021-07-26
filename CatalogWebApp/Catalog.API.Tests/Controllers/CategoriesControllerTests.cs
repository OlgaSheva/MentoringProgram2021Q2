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
	public class CategoriesControllerTests
	{
		private IEnumerable<CategoryDTO> categoryDtos = new CategoryDTO[] { new() { CategoryId = 1 }, new() { CategoryId = 2 } };
		private IEnumerable<Category> categoryViewModels = new Category[] { new() { CategoryId = 1 }, new() { CategoryId = 2 } };

		[Fact]
		public async void GeCategories_OkResult()
		{
			var logger = A.Fake<ILogger<CategoriesController>>();
			var service = A.Fake<ICategoryService>();
			var mapper = A.Fake<IMapper>();
			A.CallTo(() => service.GetAllAsync()).Returns(categoryDtos);
			A.CallTo(() => mapper.Map<IEnumerable<Category>>(A<IEnumerable<CategoryDTO>>._)).Returns(categoryViewModels);

			var controller = new CategoriesController(logger, service, mapper);
			var result = await controller.GeCategories();
			
			A.CallTo(() => service.GetAllAsync()).MustHaveHappened();
			A.CallTo(() => mapper.Map<IEnumerable<Category>>(A<IEnumerable<CategoryDTO>>._)).MustHaveHappened();
		}

		[Fact]
		public async void GeCategory_OkResult()
		{
			var logger = A.Fake<ILogger<CategoriesController>>();
			var service = A.Fake<ICategoryService>();
			var mapper = A.Fake<IMapper>();
			A.CallTo(() => service.GetAllAsync()).Returns(categoryDtos);
			A.CallTo(() => mapper.Map<IEnumerable<Category>>(A<IEnumerable<CategoryDTO>>._)).Returns(categoryViewModels);

			var controller = new CategoriesController(logger, service, mapper);
			var result = await controller.GetCategory(1);

			A.CallTo(() => service.GetAsync(A<int>._)).MustHaveHappened();
			A.CallTo(() => mapper.Map<Category>(A<CategoryDTO>._)).MustHaveHappened();
		}
	}
}
