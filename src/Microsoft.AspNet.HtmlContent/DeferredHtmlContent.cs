using System;
using System.IO;
using Microsoft.Framework.WebEncoders;

namespace Microsoft.AspNet.HtmlContent
{
    public class DeferredHtmlContent : IHtmlContent
    {
        private Action<TextWriter> _action;

        public DeferredHtmlContent(Action<TextWriter> action)
        {
            _action = action;
        }

        public void WriteTo(TextWriter writer, IHtmlEncoder encoder)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            var writer = new StringWriter();
            WriteTo(writer, new HtmlEncoder());
            return writer.ToString();
        }
    }
}