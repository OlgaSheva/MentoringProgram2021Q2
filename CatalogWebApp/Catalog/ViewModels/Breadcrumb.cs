using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Catalog.ViewModels
{
    public class Breadcrumb
    {
        public Breadcrumb(string controller, string action, string title, object id)
            : this(controller, action, title)
        {
            Id = id;
        }

        public Breadcrumb(string controller, string action, string title)
        {
            Controller = controller;
            Action = action;

            if (string.IsNullOrWhiteSpace(title))
            {
                Title = Regex.Replace(
                    CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                        string.Equals(action, "Index", StringComparison.OrdinalIgnoreCase) ? controller : action), "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
            }
            else
            {
                Title = title;
            }
        }

        public string Controller { get; set; }

        public string Action { get; set; }

        public object Id { get; set; }

        public string Title { get; set; }
    }
}
