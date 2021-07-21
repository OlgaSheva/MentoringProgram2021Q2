using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Catalog.Models.ViewModels;
using Catalog.Services.Interfaces;
using Catalog.Services.Models;
using Microsoft.Extensions.Logging;

namespace Catalog.Controllers
{
    public class ProductsController : Controller
    {
        private const string MAX_COUNT = "ProductSettings:MaxShownProductCount";
        
        private readonly int maxShownProductCount;
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public ProductsController(
            IConfiguration configuration,
            ILogger<ProductsController> logger,
            IProductService productService,
            ICategoryService categoryService,
            ISupplierService supplierService,
            IMapper mapper)
        {
            maxShownProductCount = configuration != null
                ? configuration.GetValue<int>(MAX_COUNT)
                : throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _mapper = mapper;
            _logger.LogInformation($"Maximum shown product count: {maxShownProductCount}");
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProductsAsync(maxShownProductCount);
            return View(_mapper.Map<IEnumerable<ProductViewModel>>(products));
        }

        // GET: Products/Create
        public async Task<IActionResult> CreateAsync()
        {
            var products = new ProductEditViewModel
            {
                Product = new ProductDTO(),
                Categories = await _categoryService.GetAllAsync(),
                Suppliers = await _supplierService.GetAllAsync()
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
                await _productService.AddAsync(productEditViewModel.Product);
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

            var product = await _productService.GetAsync((int)id);
            if (product == null)
            {
                _logger.LogError($"Product id = {id} not found.");
                return NotFound();
            }

            var products = new ProductEditViewModel
            {
                Product = product,
                Categories = await _categoryService.GetAllAsync(),
                Suppliers = await _supplierService.GetAllAsync()
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
                _logger.LogInformation($"Editing product id = {id}");
                await _productService.EditAsync(product);
                _logger.LogInformation($"Edited product id = {id}");
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

            var product = await _productService.GetAsync((int)id);
            if (product == null)
            {
                _logger.LogError($"Product id = {id} not found.");
                return NotFound();
            }

            return View(_mapper.Map<ProductViewModel>(product));
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation($"Removing product id = {id}");
            var result = await _productService.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            
            _logger.LogInformation($"Removed product id = {id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
