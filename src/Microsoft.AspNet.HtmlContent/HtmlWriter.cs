
using System.IO;
using Microsoft.Framework.WebEncoders;

namespace Microsoft.AspNet.HtmlContent
{
    public abstract class HtmlWriter : TextWriter
    {
        public abstract IHtmlEncoder Encoder { get; }

        public abstract void Write(IHtmlContent htmlContent);
    }
}