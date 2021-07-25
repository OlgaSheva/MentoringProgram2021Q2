using Catalog.API.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Catalog.HttpClient
{
	public static class CategoryHelpers
	{
		public static void ShowCategory(List<Category> categories)
		{
			foreach (var category in categories)
			{
				Console.WriteLine($"Name: {category.CategoryName}, Description: {category.Description}");
			}
		}

		public static async Task<List<Category>> GetCategoriesAsync(System.Net.Http.HttpClient client, string path)
		{
			List<Category> categories = new List<Category>();
			HttpResponseMessage response = await client.GetAsync(path);
			if (response.IsSuccessStatusCode)
			{
				categories = await response.Content.ReadAsAsync<List<Category>>();
			}

			return categories;
		}
	}
}
