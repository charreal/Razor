using System;
using System.Text;
using Microsoft.Framework.WebEncoders;

namespace Microsoft.AspNet.HtmlContent
{
    public class BufferedHtmlWriter : HtmlWriter
    {
        public override IHtmlEncoder Encoder
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Encoding Encoding
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void Write(IHtmlContent htmlContent)
        {
            throw new NotImplementedException();
        }

        public override void Write(char value)
        {
            throw null;
        }
    }
}