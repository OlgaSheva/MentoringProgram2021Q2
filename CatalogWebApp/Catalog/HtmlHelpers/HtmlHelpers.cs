using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Catalog.HtmlHelpers
{
    public static class HtmlHelpers
    {
        private const string IMAGE_LINK = "<a href=\"Image/{0}\">{1}</a>";

        public static IHtmlContent NorthwindImageLink(this IHtmlHelper htmlHelper, int imageId, string linkText)
            => new HtmlString(string.Format(IMAGE_LINK, imageId, linkText));
    }
}
