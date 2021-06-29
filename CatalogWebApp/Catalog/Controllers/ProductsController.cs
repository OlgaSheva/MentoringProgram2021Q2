using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Catalog.Models;
using Catalog.Models.Northwind;
using Microsoft.Extensions.Logging;

namespace Catalog.Controllers
{
    public class ProductsController : Controller
    {
        private const string MAX_COUNT = "ProductSettings:MaxShownProductCount";

        private readonly NorthwindContext _context;
        private readonly int maxShownProductCount;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(NorthwindContext context, IConfiguration configuration, ILogger<ProductsController> logger)
        {
            _context = context;
            maxShownProductCount = configuration != null
                ? configuration.GetValue<int>(MAX_COUNT)
                : throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
            _logger.LogInformation($"Maximum shown product count: {maxShownProductCount}");
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return maxShownProductCount == 0
                ? View(await _context.Products.Include(p => p.Supplier).Include(p => p.Category).ToListAsync())
                : View(await _context.Products.Take(maxShownProductCount).Include(p => p.Supplier).Include(p => p.Category).ToListAsync());
        }

        // GET: Products/Create
        public async Task<IActionResult> CreateAsync()
        {
            var products = new ProductEditViewModel
            {
                Product = new Product(),
                Categories = await _context.Categories.ToListAsync(),
                Suppliers = await _context.Suppliers.ToListAsync()
            };
            return View(products);
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductEditViewModel productEditViewModel)
        {
            _logger.LogInformation($"Creating product id = {productEditViewModel.Product.ProductId}");
            if (ModelState.IsValid)
            {
                await _context.AddAsync(productEditViewModel.Product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation($"Created product id = {productEditViewModel.Product.ProductId}");
            return View(productEditViewModel);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogError($"Product id can not be null.");
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                _logger.LogError($"Product id = {id} not found.");
                return NotFound();
            }

            var products = new ProductEditViewModel
            {
                Product = product,
                Categories = await _context.Categories.ToListAsync(),
                Suppliers = await _context.Suppliers.ToListAsync()
            };
            return View(products);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditViewModel productEditViewModel)
        {
            var product = productEditViewModel.Product;
            if (id != product.ProductId)
            {
                _logger.LogError($"Product id = {id} not found.");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation($"Editing product id = {id}");
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Edited product id = {id}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productEditViewModel);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogError($"Product id can not be null.");
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                _logger.LogError($"Product id = {id} not found.");
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orders = _context.OrderDetails
                .Where(o => o.ProductId == id);
            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            _logger.LogInformation($"Removing product id = {id}");
            _context.OrderDetails.RemoveRange(orders);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Removed product id = {id}");
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
