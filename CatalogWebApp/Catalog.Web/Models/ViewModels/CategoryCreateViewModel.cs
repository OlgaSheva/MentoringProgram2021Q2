using Microsoft.AspNetCore.Http;

namespace Catalog.ViewModels
{
    public class CategoryCreateViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

        public IFormFile FilePicture { get; set; }
    }
}
