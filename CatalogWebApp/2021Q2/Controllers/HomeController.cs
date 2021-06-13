using _2021Q2.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;

namespace _2021Q2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _logger.LogInformation($"application location - {Directory.GetCurrentDirectory()}");
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            string exceptionMessage = $"{exceptionHandlerPathFeature.Error.Message}";
            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            {
                exceptionMessage = "File error thrown";
            }

            if (exceptionHandlerPathFeature?.Path == "/index")
            {
                exceptionMessage += " from home page";
            }

            string requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            _logger.LogError($"Request ID: {requestId}, exception message: {exceptionMessage}");
            return View(new ErrorViewModel { RequestId = requestId, ErrorMessage = exceptionMessage });
        }
    }
}
