using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.DAL;
using Catalog.DAL.Models;
using Catalog.Services.Interfaces;
using Catalog.Services.Mapper;
using Catalog.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly NorthwindContext _context;

        public CategoryService(NorthwindContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var categories = await _context.Categories.ToArrayAsync();
            return Mapping.Mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            return Mapping.Mapper.Map<CategoryDTO>(category);
        }

        public async Task<int> AddAsync(CategoryDTO category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            await _context.AddAsync(Mapping.Mapper.Map<Category>(category));
            await _context.SaveChangesAsync();
            return category.CategoryId;
        }

        public async Task<bool> EditAsync(CategoryDTO category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            var dbCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);
            if (dbCategory == null)
            {
                return false;
            }

            CopyFields(category, dbCategory);
            await _context.SaveChangesAsync();
            return true;
        }

        private static void CopyFields(CategoryDTO from, Category to)
        {
            to.CategoryName = from.CategoryName;
            to.Description = from.Description;
            to.Picture = from.Picture;
        }
    }
}
