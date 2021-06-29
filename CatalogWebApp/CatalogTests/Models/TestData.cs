using System.Collections.Generic;
using System.Linq;
using Catalog.Models.Northwind;

namespace CatalogTests.Models
{
    class TestData
    {
        public static IQueryable<Category> Categories =>
            new List<Category>
            {
                new Category() { CategoryId = 1, CategoryName = "category1" },
                new Category() { CategoryId = 2, CategoryName = "category2" }
            }
            .AsQueryable();

        public static IQueryable<Product> Products =>
            new List<Product>
                {
                    new Product() { CategoryId = 1, ProductId = 1, SupplierId = 1, ProductName = "testName", QuantityPerUnit = "quantity" },
                    new Product() { CategoryId = 2, ProductId = 2, ProductName = "testName2", QuantityPerUnit = "quantity2" }
                }
                .AsQueryable();

        public static IQueryable<Supplier> Suppliers =>
            new List<Supplier>
                {
                    new Supplier() {SupplierId = 1}
                }
                .AsQueryable();
    }
}
