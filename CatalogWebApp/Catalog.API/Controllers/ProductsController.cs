using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.API.Models;
using Catalog.Services.Interfaces;
using Catalog.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public ProductsController(
            ILogger<ProductsController> logger,
            IProductService productService,
            ICategoryService categoryService,
            ISupplierService supplierService,
            IMapper mapper)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _mapper = mapper;
        }

        // GET: api/Products
        [HttpGet]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        public async Task<IEnumerable<Product>> Get()
        {
            var productDtos = await _productService.GetProductsAsync();
            return _mapper.Map<IEnumerable<Product>>(productDtos);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProduct(int? id)
        {
            if (id == null)
            {
                _logger.LogError($"Product id can not be null.");
                return NotFound();
            }

            var productDto = await _productService.GetAsync((int)id);
            if (productDto == null)
            {
                return NotFound();
            }

            return _mapper.Map<Product>(productDto);
        }

        // POST api/Products
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Post([FromForm] Product product)
        {
	        if (product == null)
	        {
		        _logger.LogError($"Product can not be null.");
		        return NotFound();
	        }

	        var productDto = _mapper.Map<ProductDTO>(product);
	        productDto.Supplier = await _supplierService.GetAsync(product.SupplierName);
	        productDto.SupplierId = productDto.Supplier.SupplierId;
	        productDto.Category = await _categoryService.GetAsync(product.CategoryName);
	        productDto.CategoryId = productDto.Category.CategoryId;

            await _productService.AddAsync(productDto);
	        return Ok();
        }

        // PUT api/Products/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, [FromForm] Product product)
        {
	        if (product == null)
	        {
		        _logger.LogError($"Product can not be null.");
		        return NotFound();
	        }

	        var existProduct = await _productService.GetAsync(product.ProductId);
	        if (existProduct == null)
	        {
		        _logger.LogError("Product doesn't exist.");
		        return NotFound();
	        }

            var productDto = _mapper.Map<ProductDTO>(product);
	        productDto.Supplier = await _supplierService.GetAsync(product.SupplierName);
	        productDto.SupplierId = productDto.Supplier.SupplierId;
	        productDto.Category = await _categoryService.GetAsync(product.CategoryName);
	        productDto.CategoryId = productDto.Category.CategoryId;

	        await _productService.EditAsync(productDto);
	        return Ok();
        }

        // DELETE api/Products/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
	        var product = await _productService.GetAsync(id);
	        if (product == null)
	        {
                _logger.LogError("Product doesn't exist.");
		        return NotFound();
	        }

	        await _productService.DeleteAsync(id);
	        return Ok();
        }
    }
}
