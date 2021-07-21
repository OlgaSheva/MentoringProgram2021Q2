using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Catalog.Services.Interfaces;
using Catalog.Services.Models;
using Microsoft.Extensions.Logging;
using Catalog.ViewModels;

namespace Catalog.Controllers
{
    public class CategoriesController : Controller
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

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<CategoryViewModel>>(await _categoryService.GetAllAsync()));
        }

        [Route("Image/{id?}")]
        [Route("{controller}/{action}/{id?}")]
        public async Task<IActionResult> Image([FromRoute] int? id)
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

            var picture = category.Picture.Skip(78).ToArray();
            return File(picture, "image/bmp");
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel categoryViewModel)
        {
            _logger.LogInformation($"Creating category id = {categoryViewModel.CategoryId}");
            var category = new CategoryDTO()
            {
                CategoryId = categoryViewModel.CategoryId,
                CategoryName = categoryViewModel.CategoryName,
                Description = categoryViewModel.Description
            };
            if (categoryViewModel.FilePicture != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(categoryViewModel.FilePicture.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)categoryViewModel.FilePicture.Length);
                }

                category.Picture = imageData;
            }

            if (ModelState.IsValid)
            {
                await _categoryService.AddAsync(category);
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation($"Created category id = {category.CategoryId}");
            return View(categoryViewModel);
        }
    }
}
