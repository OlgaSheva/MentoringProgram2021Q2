using System.Collections.Generic;
using Catalog.Models.Northwind;

namespace Catalog.ViewModels
{
    public class ProductEditViewModel
    {
        public Product Product { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<Supplier> Suppliers { get; set; }
    }
}
