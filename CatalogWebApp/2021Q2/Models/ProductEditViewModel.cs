using _2021Q2.Models.Northwind;
using System.Collections.Generic;

namespace _2021Q2.Models
{
    public class ProductEditViewModel
    {
        public Product Product { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public IEnumerable<Supplier> Suppliers { get; set; }
    }
}
