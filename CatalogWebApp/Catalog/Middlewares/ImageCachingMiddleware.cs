using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using Catalog.Interfaces;

namespace Catalog.Middlewares
{
    public class ImageCachingMiddleware
    {
        private const string ContentType = "image/bmp";
        private readonly RequestDelegate _next;
        private readonly ICachePictureService _cachePictureService;

        public ImageCachingMiddleware(RequestDelegate next, ICachePictureService cachePictureService)
        {
            _next = next;
            _cachePictureService = cachePictureService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var id = int.Parse((Regex.Match(httpContext.Request.Path.Value, @"Image/(?'id'\d+)")).Groups["id"].Value);
            var cache = await _cachePictureService.GetCachedData(id);
            if (cache != null)
            {
                httpContext.Response.ContentType = ContentType;
                await httpContext.Response.Body.WriteAsync(cache.ToArray());
                return;
            }
            else
            {
                await using MemoryStream memoryStream = new MemoryStream();
                Stream originalBody = httpContext.Response.Body;
                httpContext.Response.Body = memoryStream;
                try
                {
                    await _next(httpContext);

                    if (httpContext.Response.ContentType == ContentType)
                    {
                        await _cachePictureService.Cache(id, memoryStream);
                    }
                }
                finally
                {
                    memoryStream.Seek(0, SeekOrigin.Begin); 
                    await memoryStream.CopyToAsync(originalBody);
                }
            }
        }
    }
    
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseImageCashing(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageCachingMiddleware>();
        }
    }
}
