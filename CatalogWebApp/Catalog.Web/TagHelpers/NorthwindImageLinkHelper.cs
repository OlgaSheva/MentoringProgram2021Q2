using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Catalog.TagHelpers
{
    [HtmlTargetElement("a", Attributes = NorthwindIdAttributeName)]
    public class NorthwindImageLinkHelper : TagHelper
    {
        private const string NorthwindIdAttributeName = "northwind-id";
        private const string IMAGE_ROUTE_NAME = "Image";
        private const string HREF = "href";

        //<a northwind-id="imageId">Link text</a>
        [HtmlAttributeName(NorthwindIdAttributeName)]
        public string NorthwindId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var link = $"{IMAGE_ROUTE_NAME}/{NorthwindId}";
            output.Attributes.SetAttribute(HREF, link);
        }
    }
}

