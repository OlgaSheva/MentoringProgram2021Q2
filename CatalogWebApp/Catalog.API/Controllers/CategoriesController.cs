using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.API.Models;
using Catalog.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(
            ILogger<CategoriesController> logger,
            ICategoryService categoryService,
            IMapper mapper)
        {
            _logger = logger;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all categories.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GeCategories()
        {
            var categoriesDto = await _categoryService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<Category>>(categoriesDto));
        }

        /// <summary>
        /// Gets a specific category.
        /// </summary>
        /// <param name="categoryId">The categoryId.</param>
        [HttpGet("{categoryId}")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> GetCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                _logger.LogError($"Category categoryId can not be null.");
                return NotFound();
            }

            var category = await _categoryService.GetAsync((int)categoryId);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Category>(category));
        }

        /// <summary>
        /// Updates a specific category.
        /// </summary>
        /// <param name="categoryIdtegoryId.</param>
        /// <param name="file">The picture file.</param>
        [HttpPut("{categoryId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PutCategory(int categoryId, IFormFile file)
        {
	        var category = await _categoryService.GetAsync((int)categoryId);
	        if (file != null)
	        {
		        byte[] imageData = null;
		        using (var binaryReader = new BinaryReader(file.OpenReadStream()))
		        {
			        imageData = binaryReader.ReadBytes((int)file.Length);
		        }

		        category.Picture = imageData;
	        }

	        await _categoryService.EditAsync(category);
	        return Ok();
        }
    }
}
