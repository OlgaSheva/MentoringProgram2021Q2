using System;
using System.Collections.Generic;
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
        public async Task<IEnumerable<Product>> Get()
        {
            var productDtos = await _productService.GetProductsAsync();
            return _mapper.Map<IEnumerable<Product>>(productDtos);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int? id)
        {
            if (id == null)
            {
                _logger.LogError($"Category id can not be null.");
                return NotFound();
            }

            var productDto = await _productService.GetAsync((int)id);
            if (productDto == null)
            {
                return NotFound();
            }

            return _mapper.Map<Product>(productDto);
        }
    }
}
