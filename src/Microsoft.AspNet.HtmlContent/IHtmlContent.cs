using System.IO;
using Microsoft.Framework.WebEncoders;

namespace Microsoft.AspNet.HtmlContent
{
    public interface IHtmlContent
    {
        void WriteTo(TextWriter writer, IHtmlEncoder encoder);
    }
}