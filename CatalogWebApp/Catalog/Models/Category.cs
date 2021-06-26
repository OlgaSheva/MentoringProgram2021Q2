using System;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Catalog.Models.Northwind
{
    public partial class Category
    {
        [NotMapped] public string Base64String
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