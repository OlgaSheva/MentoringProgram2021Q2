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

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            var categoriesDto = await _categoryService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<Category>>(categoriesDto));
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> GetCategory(int? id)
        {
            if (id == null)
            {
                _logger.LogError($"Category id can not be null.");
                return NotFound();
            }

            var category = await _categoryService.GetAsync((int)id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Category>(category));
        }

        // PUT api/Categories/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, IFormFile file)
        {
	        var category = await _categoryService.GetAsync((int)id);
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
