using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Services.Models;

namespace Catalog.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProductsAsync(int maxCount = 0);

        Task<ProductDTO> GetAsync(int id);

        Task<int> AddAsync(ProductDTO product);

        Task<bool> EditAsync(ProductDTO product);

        Task<bool> DeleteAsync(int id);
    }
}
