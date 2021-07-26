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
    [Produces("application/json")]
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

        /// <summary>
        /// Gets all product.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        public async Task<IEnumerable<Product>> Get()
        {
            var productDtos = await _productService.GetProductsAsync();
            return _mapper.Map<IEnumerable<Product>>(productDtos);
        }

        /// <summary>
        /// Gets a specific product.
        /// </summary>
        /// <param name="productId">The createProductId.</param>
        [HttpGet("{createProductId}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProduct(int? productId)
        {
            if (productId == null)
            {
                _logger.LogError($"Product createProductId can not be null.");
                return NotFound();
            }

            var productDto = await _productService.GetAsync((int)productId);
            if (productDto == null)
            {
                return NotFound();
            }

            return _mapper.Map<Product>(productDto);
        }

        /// <summary>
        /// Creates a product.
        /// </summary>
        /// <param name="product">The product.</param>
        [HttpPost("{createProductId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Post(int createProductId, [FromForm] Product product)
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

        /// <summary>
        /// Updates the product.
        /// </summary>
        /// <param name="updateProductId">The createProductId.</param>
        /// <param name="product">The product.</param>
        [HttpPut("{createProductId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int updateProductId, [FromForm] Product product)
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

        /// <summary>
        /// Deletes a specific product.
        /// </summary>
        /// <param name="deleteProductId">The createProductId.</param>
        [HttpDelete("{createProductId}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int deleteProductId)
        {
	        var product = await _productService.GetAsync(deleteProductId);
	        if (product == null)
	        {
                _logger.LogError("Product doesn't exist.");
		        return NotFound();
	        }

	        await _productService.DeleteAsync(deleteProductId);
	        return Ok();
        }
    }
}
