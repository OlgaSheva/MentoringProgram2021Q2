using System;
using Catalog.Filters;
using Catalog.Middlewares;
using Catalog.Models;
using Catalog.Models.Northwind;
using Catalog.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Catalog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddHttpContextAccessor();

            services.AddDbContext<NorthwindContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            if (Configuration.GetSection("LogActionFilterOn").Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                services.AddControllersWithViews(options =>
                    options.Filters.Add(new LogActionFilter(new Logger<LogActionFilter>(new LoggerFactory()))));
            }

            services.AddResponseCaching();
            services.AddTransient<ICashPictureService, FileCashPictureService>()
                .Configure<CashSettings>(Configuration.GetSection("CashSettings"));

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Configuration.GetSection("Logging"));

            if (env.IsDevelopment())
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseResponseCaching();
            app.UseWhen(context => context.Request.Path.Value.Contains("Image/"), appBuilder =>
            {
                appBuilder.UseImageCashing();
            });
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
