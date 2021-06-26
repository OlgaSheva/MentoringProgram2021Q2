using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Catalog.Models.Northwind;
using Microsoft.Extensions.Logging;

namespace Catalog.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly NorthwindContext _context;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(NorthwindContext context, ILogger<CategoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        public async Task<IActionResult> Image([FromRoute]int? id)
        {
            if (_context.Categories.Any(c => c.CategoryId == id))
            {
                return View(await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id));
            }

            throw new ArgumentException($"Picture with id = {id} does not exist");
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel categoryViewModel)
        {
            _logger.LogInformation($"Creating category id = {categoryViewModel.CategoryId}");
            var category = new Category()
            {
                CategoryId = categoryViewModel.CategoryId,
                CategoryName = categoryViewModel.CategoryName,
                Description = categoryViewModel.Description
            };
            if (categoryViewModel.Picture != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(categoryViewModel.Picture.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)categoryViewModel.Picture.Length);
                }

                category.Picture = imageData;
            }

            if (ModelState.IsValid)
            {
                await _context.AddAsync(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation($"Created category id = {category.CategoryId}");
            return View(categoryViewModel);
        }
    }
}
