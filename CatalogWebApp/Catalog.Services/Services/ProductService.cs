using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.DAL;
using Catalog.DAL.Models;
using Catalog.Services.Interfaces;
using Catalog.Services.Mapper;
using Catalog.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly NorthwindContext _context;

        public ProductService(NorthwindContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsAsync(int maxCount = 0)
        {
            var categories = maxCount == 0
            ? await _context.Products.Include(p => p.Supplier).Include(p => p.Category).ToArrayAsync()
            : await _context.Products.Take(maxCount).Include(p => p.Supplier).Include(p => p.Category).ToArrayAsync();
            return Mapping.Mapper.Map<IEnumerable<ProductDTO>>(categories);
        }

        public async Task<ProductDTO> GetAsync(int id)
        {
            var product = await _context.Products.Include(p => p.Supplier).Include(p => p.Category).FirstOrDefaultAsync(c => c.ProductId == id);
            return Mapping.Mapper.Map<ProductDTO>(product);
        }

        public async Task<int> AddAsync(ProductDTO product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            await _context.AddAsync(Mapping.Mapper.Map<Product>(product));
            await _context.SaveChangesAsync();
            return product.ProductId;
        }

        public async Task<bool> EditAsync(ProductDTO product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var dbProduct = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == product.ProductId);
            if (dbProduct == null)
            {
                return false;
            }

            CopyFields(product, dbProduct);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var orders = _context.OrderDetails.Where(o => o.ProductId == id);
            var product = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == id);
            if (product == null)
            {
                return false;
            }

            _context.OrderDetails.RemoveRange(orders);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        private static void CopyFields(ProductDTO from, Product to)
        {
            to.CategoryId = from.CategoryId;
            to.Discontinued = from.Discontinued;
            to.ProductName = from.ProductName;
            to.QuantityPerUnit = from.QuantityPerUnit;
            to.SupplierId = from.SupplierId;
            to.ReorderLevel = from.ReorderLevel;
            to.UnitPrice = from.UnitPrice;
            to.UnitsInStock = from.UnitsInStock;
            to.UnitsOnOrder = from.UnitsOnOrder;
        }
    }
}
