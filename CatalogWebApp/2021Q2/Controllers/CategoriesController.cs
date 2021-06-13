using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _2021Q2.Models.Northwind;
using Microsoft.Extensions.Logging;

namespace _2021Q2.Controllers
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

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,Description")] Category category)
        {
            _logger.LogInformation($"Creating category id = {category.CategoryId}");
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            _logger.LogInformation($"Created category id = {category.CategoryId}");
            return View(category);
        }
    }
}
