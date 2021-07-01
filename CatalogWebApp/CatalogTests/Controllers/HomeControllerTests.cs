using System;
using Catalog.Controllers;
using Catalog.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CatalogTests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_ReturnsAIViewResult()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(logger.Object, httpContextAccessorMock.Object);

            var result = controller.Index();

            Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void Error_ReturnsAIViewResult_WithErrorViewModel()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var httpContextMock = new Mock<HttpContext>();
            var featureCollectionMock = new Mock<IFeatureCollection>();
            var exceptionHandlerPathFeatureMock = new Mock<IExceptionHandlerPathFeature>();

            httpContextAccessorMock.Setup(ca => ca.HttpContext).Returns(httpContextMock.Object);
            httpContextMock.Setup(c => c.Features).Returns(featureCollectionMock.Object);
            httpContextMock.Setup(c => c.TraceIdentifier).Returns("1");
            featureCollectionMock.Setup(fc => fc.Get<IExceptionHandlerFeature>()).Returns(exceptionHandlerPathFeatureMock.Object);
            
            var logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(logger.Object, httpContextAccessorMock.Object);

            Exception thrownException;
            try
            {
                throw new ApplicationException("A thrown application exception");
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            exceptionHandlerPathFeatureMock.Setup(ehf => ehf.Error).Returns(thrownException);
            
            var result = controller.Error();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(viewResult.ViewData.Model);
            Assert.Equal("1", model.RequestId);
            Assert.Equal("A thrown application exception", model.ErrorMessage);
            logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Request ID: 1, exception message: A thrown application exception", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
