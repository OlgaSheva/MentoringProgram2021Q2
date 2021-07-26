using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Catalog.API.Models;

namespace Catalog.HttpClient
{
	public static class ProductHelper
	{
		public static void ShowProduct(List<Product> products)
		{
			foreach (var product in products)
			{
				Console.WriteLine($"Name: {product.ProductName}, Category: {product.CategoryName}, Supplier: {product.SupplierName}");
			}
		}

		public static async Task<List<Product>> GetProductsAsync(System.Net.Http.HttpClient client, string path)
		{
			List<Product> products = new List<Product>();
			HttpResponseMessage response = await client.GetAsync(path);
			if (response.IsSuccessStatusCode)
			{
				products = await response.Content.ReadAsAsync<List<Product>>();
			}

			return products;
		}
	}
}
