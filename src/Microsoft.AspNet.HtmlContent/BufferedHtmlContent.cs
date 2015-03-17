
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Framework.WebEncoders;

namespace Microsoft.AspNet.HtmlContent
{
    public class BufferedHtmlContent : IHtmlContent, IEnumerable<object>
    {
        private List<object> _entries = new List<object>();

        public void Append(string value)
        {
            if (value != null)
            {
                _entries.Add(value);
            }
        }

        public void Append(IHtmlContent htmlContent)
        {
            var bufferedHtmlContext = htmlContent as BufferedHtmlContent;
            if (bufferedHtmlContext != null)
            {
                _entries.AddRange(bufferedHtmlContext._entries);
            }
            else if (htmlContent != null)
            {
                _entries.Add(htmlContent);
            }
        }

        public void AppendEncoded(string encodedValue)
        {
            if (encodedValue != null)
            {
                _entries.Add(new StringHtmlContent(encodedValue));
            }
        }

        public void Clear()
        {
            _entries.Clear();
        }

        public void WriteTo(TextWriter writer, IHtmlEncoder encoder)
        {
            foreach (var entry in _entries)
            {
                var entryAsString = entry as string;
                if (entryAsString != null)
                {
                    encoder.HtmlEncode(entryAsString, writer);

                }
                else
                {
                    var entryAsHtmlContent = (IHtmlContent)entry;
                    entryAsHtmlContent.WriteTo(writer, encoder);
                }
            }
        }

        public override string ToString()
        {
            var writer = new StringWriter();
            WriteTo(writer, new HtmlEncoder());
            return writer.ToString();
        }

        public IEnumerator<object> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}