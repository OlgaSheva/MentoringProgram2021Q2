using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        public string Description { get; set; }

        public byte[] Picture { get; set; }
    }
}
