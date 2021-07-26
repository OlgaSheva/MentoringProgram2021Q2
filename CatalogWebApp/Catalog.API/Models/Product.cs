using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        public string QuantityPerUnit { get; set; }

        [Required]
        public decimal? UnitPrice { get; set; }

        [Required]
        public short? UnitsInStock { get; set; }

        [Required]
        public short? UnitsOnOrder { get; set; }

        [Required]
        public short? ReorderLevel { get; set; }

        [DefaultValue(false)]
        public bool Discontinued { get; set; }

        public string CategoryName { get; set; }

        public string SupplierName { get; set; }
	}
}
