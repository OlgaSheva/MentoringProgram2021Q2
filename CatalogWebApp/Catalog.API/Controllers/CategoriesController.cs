using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.API.Models;
using Catalog.Services.Interfaces;
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
        public async Task<IEnumerable<Category>> Get()
        {
            var categoriesDto = await _categoryService.GetAllAsync();
            return _mapper.Map<IEnumerable<Category>>(categoriesDto);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
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

            return _mapper.Map<Category>(category);
        }

        // POST api/Categories
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/Categories/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/Categories/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
