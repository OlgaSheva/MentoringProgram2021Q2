using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

        public IFormFile FilePicture { get; set; }

        [NotMapped]
        public string Base64String
        {
            get
            {
                // Note. Please note that test data for Northwind Categories contain broken images (it’s BMP pictures, but the first 78 bytes is garbage) 
                if (Picture.Length == 10746)
                {
                    return Convert.ToBase64String(Picture, 78, Picture.Length - 78);
                }
                else
                {
                    return Convert.ToBase64String(Picture);
                }
            }
        }
    }
}
