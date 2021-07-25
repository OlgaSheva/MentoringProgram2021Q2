using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Catalog.HttpClient
{
	class Program
	{
		static System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

		static void Main()
		{
			RunAsync().GetAwaiter().GetResult();
		}

		static async Task RunAsync()
		{
			client.BaseAddress = new Uri("http://localhost:15457/api/");
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(
				new MediaTypeWithQualityHeaderValue("application/json"));

			try
			{
				var products = await ProductHelper.GetProductsAsync(client, "products");
				ProductHelper.ShowProduct(products);

				var categories = await CategoryHelpers.GetCategoriesAsync(client, "categories");
				CategoryHelpers.ShowCategory(categories);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Console.ReadLine();
		}
	}
}
