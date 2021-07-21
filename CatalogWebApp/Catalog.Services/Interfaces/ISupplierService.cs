using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Services.Models;

namespace Catalog.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDTO>> GetAllAsync();
    }
}
