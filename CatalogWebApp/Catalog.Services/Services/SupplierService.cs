using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.DAL;
using Catalog.Services.Interfaces;
using Catalog.Services.Mapper;
using Catalog.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Services.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly NorthwindContext _context;

        public SupplierService(NorthwindContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<SupplierDTO>> GetAllAsync()
        {
            var suppliers = await _context.Suppliers.ToArrayAsync();
            return Mapping.Mapper.Map<IEnumerable<SupplierDTO>>(suppliers);
        }

        public async Task<SupplierDTO> GetAsync(string name)
        {
	        var supplier = await _context.Suppliers.FirstOrDefaultAsync(c => c.CompanyName == name);
	        return Mapping.Mapper.Map<SupplierDTO>(supplier);
        }
    }
}
