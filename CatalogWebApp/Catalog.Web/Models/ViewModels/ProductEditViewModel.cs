using System.Collections.Generic;
using Catalog.Services.Models;

namespace Catalog.Models.ViewModels
{
    public class ProductEditViewModel
    {
        public ProductDTO Product { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }

        public IEnumerable<SupplierDTO> Suppliers { get; set; }
    }
}
