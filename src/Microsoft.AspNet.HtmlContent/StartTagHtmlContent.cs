using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Framework.WebEncoders;

namespace Microsoft.AspNet.HtmlContent
{
    public class StartTagHtmlContent : IHtmlContent
    {
        private string _tag;
        private IDictionary<string, string> _attributes;
        private bool _isSelfClosing;

        public StartTagHtmlContent(string tag)
            : this(tag, attributes: null, isSelfClosing: false)
        {
        }

        public StartTagHtmlContent(string tag, bool isSelfClosing)
            : this(tag, attributes: null, isSelfClosing: isSelfClosing)
        {
        }

        public StartTagHtmlContent(string tag, IDictionary<string, string> attributes)
            : this(tag, attributes, isSelfClosing: false)
        {
        }

        public StartTagHtmlContent(string tag, IDictionary<string, string> attributes, bool isSelfClosing)
        {
            _tag = tag;
            _attributes = attributes;
            _isSelfClosing = isSelfClosing;
        }

        public void WriteTo(TextWriter writer, IHtmlEncoder encoder)
        {
            writer.Write('<');
            writer.Write(_tag);

            if (_attributes != null)
            {
                foreach (var attribute in _attributes)
                {
                    writer.Write(' ');
                    writer.Write(attribute.Key);
                    writer.Write("=\"");
                    encoder.HtmlEncode(attribute.Value, writer);
                    writer.Write('"');
                }
            }

            if (_isSelfClosing)
            {
                writer.Write(" />");
            }
            else
            {
                writer.Write('>');
            }
        }

        public override string ToString()
        {
            var writer = new StringWriter();
            WriteTo(writer, new HtmlEncoder());
            return writer.ToString();
        }
    }
}