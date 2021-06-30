using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Catalog.Services;
using System.IO;

namespace Catalog.Middlewares
{
    public class ImageCashingMiddleware
    {
        private const string ContentType = "image/bmp";
        private readonly RequestDelegate _next;
        private readonly ICashPictureService _cashPictureService;

        public ImageCashingMiddleware(RequestDelegate next, ICashPictureService cashPictureService)
        {
            _next = next;
            _cashPictureService = cashPictureService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var id = int.Parse((Regex.Match(httpContext.Request.Path.Value, @"Image/(?'id'\d+)")).Groups["id"].Value);
            var cashExists = await _cashPictureService.TryCashedDataAsync(id, httpContext.Response);
            if (httpContext.Response.ContentType == ContentType && cashExists)
            {
                return;
            }
            else
            {
                Stream originalBody = httpContext.Response.Body;
                try
                {
                    await _next(httpContext);

                    if (httpContext.Response.ContentType == ContentType)
                    {
                        await _cashPictureService.Cash(id, originalBody);
                    }
                }
                finally
                {
                    httpContext.Response.Body = originalBody;
                }
            }
        }
    }
    
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseImageCashing(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageCashingMiddleware>();
        }
    }
}
