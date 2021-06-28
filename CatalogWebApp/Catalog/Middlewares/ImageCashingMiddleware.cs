using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Catalog.Models;
using Catalog.Services;
using Microsoft.AspNetCore.ResponseCaching;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Catalog.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ImageCashingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICashPictureService _cashPictureService;
        private readonly CashSettings _cashSettings;

        public ImageCashingMiddleware(RequestDelegate next, ICashPictureService cashPictureService, IOptions<CashSettings> configuration)
        {
            _next = next;
            _cashPictureService = cashPictureService;
            _cashSettings = configuration.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.GetTypedHeaders().CacheControl =
                new CacheControlHeaderValue()
                {
                    MaxAge = TimeSpan.FromSeconds(_cashSettings.CacheExpirationTimeInSec)
                };
            var responseCachingFeature = context.Features.Get<IResponseCachingFeature>();
            if (responseCachingFeature != null)
            {
                responseCachingFeature.VaryByQueryKeys = new[] { "id" };
            }

            await _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseImageCashing(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageCashingMiddleware>();
        }
    }
}
