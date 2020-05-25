using Markdig;
using Microsoft.AspNetCore.Html;

namespace PUTSWeb.Helpers
{
    public static class Markdown
    {
        public static string Parse(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();
            return Markdig.Markdown.ToHtml(markdown, pipeline);
        }

        public static HtmlString ParseHtmlString(string markdown)
        {
            return new HtmlString(Parse(markdown));
        }
    }
}
