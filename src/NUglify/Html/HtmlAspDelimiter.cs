namespace NUglify.Html
{
    /// <summary>
    /// A HTML comment node.
    /// </summary>
    /// <seealso cref="HtmlTextBase" />
    public class HtmlAspDelimiter : HtmlTextBase
    {
        public override string ToString()
        {
            return $"html-asp-delimiter: <!--{Slice}-->";
        }
    }
}