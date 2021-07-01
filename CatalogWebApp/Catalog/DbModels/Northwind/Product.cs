using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Catalog.Models.Northwind
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(20, MinimumLength = 3,
            ErrorMessage = "Product name must be between 3 and 20 characters long")]
        public string ProductName { get; set; }

        public int? SupplierId { get; set; }

        public int? CategoryId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(20, MinimumLength = 3,
            ErrorMessage = "Quantity per unit must be between 3 and 20 characters long")]
        public string QuantityPerUnit { get; set; }

        [Range(0, 999.99)]
        public decimal? UnitPrice { get; set; }

        [Range(0, 100)]
        public short? UnitsInStock { get; set; }

        [Range(0, 100)]
        public short? UnitsOnOrder { get; set; }

        [Range(0, 100)]
        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public virtual Category Category { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
