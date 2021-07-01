using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using Catalog.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Catalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger.LogInformation($"application location - {Directory.GetCurrentDirectory()}");
        }

        public IActionResult Index()
        {
            throw new Exception();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = _httpContextAccessor.HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            string exceptionMessage = $"{exceptionHandlerPathFeature.Error.Message}";
            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            {
                exceptionMessage = "File error thrown";
            }

            if (exceptionHandlerPathFeature?.Path == "/index")
            {
                exceptionMessage += " from home page";
            }

            string requestId = Activity.Current?.Id ?? _httpContextAccessor.HttpContext.TraceIdentifier;
            _logger.LogError($"Request ID: {requestId}, exception message: {exceptionMessage}");
            return View(new ViewModels.ErrorViewModel { RequestId = requestId, ErrorMessage = exceptionMessage });
        }
    }
}
