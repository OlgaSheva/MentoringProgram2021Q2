using Microsoft.Extensions.DependencyInjection;
using System;
using Catalog.DAL;
using Catalog.Services.Interfaces;
using Catalog.Services.Services;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDAL(this IServiceCollection services, string connectionString)
        {
            services = services ?? throw new ArgumentNullException(nameof(services));
            connectionString = !string.IsNullOrWhiteSpace(connectionString) ? connectionString : throw new ArgumentException("ConnectionString is empty or null or whitespace", nameof(connectionString));

            return services
                .AddDbContext<NorthwindContext>(options => options.UseSqlServer(connectionString))
                .AddScoped<IProductService, ProductService>()
                .AddScoped<ICategoryService, CategoryService>()
                .AddScoped<ISupplierService, SupplierService>();
        }
    }
}
