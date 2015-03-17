
using System;
using System.IO;
using System.Text;
using Microsoft.Framework.WebEncoders;

namespace Microsoft.AspNet.HtmlContent
{
    public class OutputHtmlWriter : HtmlWriter
    {
        private readonly TextWriter _output;
        private readonly IHtmlEncoder _encoder;

        public OutputHtmlWriter(TextWriter output, IHtmlEncoder encoder)
        {
            _output = output;
            _encoder = encoder;
        }

        public override void Write(IHtmlContent htmlContent)
        {
            htmlContent.WriteTo(_output, _encoder);
        }

        public override Encoding Encoding => _output.Encoding;

        public override IHtmlEncoder Encoder => _encoder;

        public override void Write(char value)
        {
            _output.Write(value);
        }
    }
}