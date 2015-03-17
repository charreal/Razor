// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using Microsoft.AspNet.HtmlContent;
using Microsoft.Framework.WebEncoders;
using System;

namespace Microsoft.AspNet.Razor.Runtime.TagHelpers
{
    /// <summary>
    /// Default concrete <see cref="TagHelperContent"/>.
    /// </summary>
    public class DefaultTagHelperContent : TagHelperContent
    {
        private BufferedHtmlContent _innerContent;

        private BufferedHtmlContent InnerContent
        {
            get
            {
                if (_innerContent == null)
                {
                    _innerContent = new BufferedHtmlContent();
                }

                return _innerContent;
            }
        }

        /// <inheritdoc />
        public override bool IsModified
        {
            get
            {
                return _innerContent != null;
            }
        }

        /// <inheritdoc />
        public override bool IsWhiteSpace
        {
            get
            {
                if (_innerContent == null)
                {
                    return true;
                }

                foreach (var value in _innerContent)
                {
                    var stringValue = value as string;
                    if (stringValue == null || !string.IsNullOrWhiteSpace(stringValue))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <inheritdoc />
        public override bool IsEmpty
        {
            get
            {
                if (_innerContent == null)
                {
                    return true;
                }

                foreach (var value in _innerContent)
                {
                    var stringValue = value as string;
                    if (stringValue == null || !string.IsNullOrEmpty(stringValue))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <inheritdoc />
        public override TagHelperContent SetContent(string value)
        {
            Clear();
            Append(value);
            return this;
        }

        public override TagHelperContent SetContent(IHtmlContent htmlContent)
        {
            Clear();
            Append(htmlContent);
            return this;
        }

        /// <inheritdoc />
        public override TagHelperContent Append(string value)
        {
            InnerContent.Append(value);
            return this;
        }

        public override TagHelperContent Append(IHtmlContent htmlContent)
        {
            var defaultTagHelperContext = htmlContent as DefaultTagHelperContent;
            if (defaultTagHelperContext == null)
            {
                InnerContent.Append(htmlContent);
            }
            else
            {
                InnerContent.Append(defaultTagHelperContext._innerContent);
            }

            return this;
        }

        /// <inheritdoc />
        public override TagHelperContent Clear()
        {
            InnerContent.Clear();
            return this;
        }

        /// <inheritdoc />
        public override string GetContent()
        {
            return string.Join(string.Empty, InnerContent);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return GetContent();
        }

        /// <inheritdoc />
        public override IEnumerator<object> GetEnumerator()
        {
            // The enumerator is exposed so that SetContent(TagHelperContent) and Append(TagHelperContent)
            // can use this to iterate through the values of the buffer.
            if (_innerContent == null)
            {
                return Enumerable.Empty<object>().GetEnumerator();
            }

            return _innerContent.GetEnumerator();
        }

        public override void WriteTo(TextWriter writer, IHtmlEncoder encoder)
        {
            _innerContent.WriteTo(writer, encoder);
        }
    }
}