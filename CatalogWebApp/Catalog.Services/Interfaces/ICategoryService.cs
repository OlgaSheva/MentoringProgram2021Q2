using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Services.Models;

namespace Catalog.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync();

        Task<CategoryDTO> GetAsync(int id);

        Task<int> AddAsync(CategoryDTO category);

        Task<bool> EditAsync(CategoryDTO category);
    }
}
