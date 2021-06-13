using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _2021Q2.Models.Northwind;
using Microsoft.Extensions.Options;
using _2021Q2.Models.Configuration;
using Microsoft.Extensions.Configuration;
using _2021Q2.Models;

namespace _2021Q2.Controllers
{
    public class ProductsController : Controller
    {
        private const string MAX_COUNT = "ProductSettings:MaxShownProductCount";
        private readonly NorthwindContext _context;
        private readonly int maxShownProductCount;

        public ProductsController(NorthwindContext context, IConfiguration configuration)
        {
            _context = context;
            maxShownProductCount = configuration != null
                ? configuration.GetValue<int>(MAX_COUNT)
                : throw new ArgumentNullException(nameof(configuration));
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            if (maxShownProductCount == 0)
            {
                return View(await _context.Products.Include(p => p.Supplier).Include(p => p.Category).ToListAsync());
            }
            else
            {
                return View(await _context.Products.Take(maxShownProductCount).Include(p => p.Supplier).Include(p => p.Category).ToListAsync());
            }
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
            if (ModelState.IsValid)
            {
                _context.Add(productEditViewModel.Product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productEditViewModel.Product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
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
        public async Task<IActionResult> Edit(int id, ProductEditViewModel products)
        {
            var product = products.Product;
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
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
            return View(products);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.FindAsync(id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
