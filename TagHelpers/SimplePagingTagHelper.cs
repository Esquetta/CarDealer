using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealer.TagHelpers
{
    [HtmlTargetElement("pager")]
    public class SimplePagingTagHelper : TagHelper
    {
        [HtmlAttributeName("pager-pagesize")]
        public int pageSize { get; set; }
        [HtmlAttributeName("pager-pagecount")]
        public int pageCount { get; set; }
        [HtmlAttributeName("pager-controller")]
        public string controller { get; set; }
        [HtmlAttributeName("pager-currentpage")]
        public int currentPage { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<ul class='pagination justify-content-center'>");
            for (int i = 1; i <= pageCount; i++)
            {
                stringBuilder.AppendFormat("<li class='page-item {0}'>", i == currentPage ? "active" : "");
                stringBuilder.AppendFormat("<a class='page-link' href='/{0}?page={1}'>{2}</a>", controller, i,i);
                stringBuilder.AppendFormat("</li>");
            }
            output.Content.SetHtmlContent(stringBuilder.ToString());
            base.Process(context, output);
        }
    }
}
