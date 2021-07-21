using System.Collections.Generic;

namespace Catalog.Services.Models
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

        public virtual ICollection<ProductDTO> Products { get; set; }
    }
}
