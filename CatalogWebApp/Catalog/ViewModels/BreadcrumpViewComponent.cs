using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.ViewModels
{
    public class BreadcrumbViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Microsoft.AspNetCore.Mvc.Rendering.ViewContext viewContext)
        {
            var routeValues = viewContext?.RouteData?.Values;

            string controller = routeValues["Controller"]?.ToString();
            string action = routeValues["Action"]?.ToString();
            object id = routeValues["Id"];
            string title = viewContext.ViewData["Title"]?.ToString(); ;
            var breadcrumbs = new List<Breadcrumb>();

            breadcrumbs.Add(new Breadcrumb("Home", "Index", "Home"));
            breadcrumbs.Add(new Breadcrumb(controller, action, title, id));
            if (!string.Equals(action, "Index", StringComparison.InvariantCultureIgnoreCase))
            {
                breadcrumbs.Insert(1, new Breadcrumb(controller, "Index", controller));
            }

            return View("BreadcrumbViewComponent", breadcrumbs);
        }

    }
}
