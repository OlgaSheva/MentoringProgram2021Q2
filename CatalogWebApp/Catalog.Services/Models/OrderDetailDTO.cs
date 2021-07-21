namespace Catalog.Services.Models
{
    public class OrderDetailDTO
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public decimal UnitPrice { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        public virtual OrderDTO Order { get; set; }

        public virtual ProductDTO Product { get; set; }
    }
}
