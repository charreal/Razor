// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNet.HtmlContent;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Razor.Runtime.TagHelpers
{
    /// <summary>
    /// Class used to represent the output of an <see cref="ITagHelper"/>.
    /// </summary>
    public class TagHelperOutput
    {
        private readonly DefaultTagHelperContent _preContent;
        private readonly DefaultTagHelperContent _content;
        private readonly DefaultTagHelperContent _postContent;

        // Internal for testing
        internal TagHelperOutput(string tagName)
            : this(tagName, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase))
        {
        }

        /// <summary>
        /// Instantiates a new instance of <see cref="TagHelperOutput"/>.
        /// </summary>
        /// <param name="tagName">The HTML element's tag name.</param>
        /// <param name="attributes">The HTML attributes.</param>
        public TagHelperOutput(
            string tagName,
            [NotNull] IDictionary<string, string> attributes)
        {
            TagName = tagName;
            Attributes = new Dictionary<string, string>(attributes, StringComparer.OrdinalIgnoreCase);
            _preContent = new DefaultTagHelperContent();
            _content = new DefaultTagHelperContent();
            _postContent = new DefaultTagHelperContent();
        }

        /// <summary>
        /// The HTML element's tag name.
        /// </summary>
        /// <remarks>
        /// A whitespace or <c>null</c> value results in no start or end tag being rendered.
        /// </remarks>
        public string TagName { get; set; }

        private bool IsTagNameNullOrWhitespace => string.IsNullOrWhiteSpace(TagName);

        /// <summary>
        /// The HTML element's pre content.
        /// </summary>
        /// <remarks>Value is prepended to the <see cref="ITagHelper"/>'s final output.</remarks>
        public TagHelperContent PreContent
        {
            get
            {
                return _preContent;
            }
        }

        /// <summary>
        /// The HTML element's main content.
        /// </summary>
        /// <remarks>Value occurs in the <see cref="ITagHelper"/>'s final output after <see cref="PreContent"/> and 
        /// before <see cref="PostContent"/></remarks>
        public TagHelperContent Content
        {
            get
            {
                return _content;
            }
        }

        /// <summary>
        /// The HTML element's post content.
        /// </summary>
        /// <remarks>Value is appended to the <see cref="ITagHelper"/>'s final output.</remarks>
        public TagHelperContent PostContent
        {
            get
            {
                return _postContent;
            }
        }

        /// <summary>
        /// <c>true</c> if <see cref="Content"/> has been set, <c>false</c> otherwise.
        /// </summary>
        public bool IsContentModified
        {
            get
            {
                return Content.IsModified;
            }
        }

        /// <summary>
        /// Indicates whether or not the tag is self-closing.
        /// </summary>
        public bool SelfClosing { get; set; }

        /// <summary>
        /// The HTML element's attributes.
        /// </summary>
        public IDictionary<string, string> Attributes { get; }


        /// <summary>
        /// Generates the <see cref="TagHelperOutput"/>'s start tag.
        /// </summary>
        /// <returns><c>string.Empty</c> if <see cref="TagName"/> is <c>null</c> or whitespace. Otherwise, the
        /// <see cref="string"/> representation of the <see cref="TagHelperOutput"/>'s start tag.</returns>
        public IHtmlContent GenerateStartTag()
        {
            // Only render a start tag if the tag name is not whitespace
            if (IsTagNameNullOrWhitespace)
            {
                return StringHtmlContent.Empty;
            }

            return new StartTagHtmlContent(TagName, Attributes, SelfClosing);
        }

        /// <summary>
        /// Generates the <see cref="TagHelperOutput"/>'s <see cref="PreContent"/>.
        /// </summary>
        /// <returns><c>null</c> if <see cref="TagName"/> is not <c>null</c> or whitespace
        /// and <see cref="SelfClosing"/> is <c>true</c>.
        /// Otherwise, an <see cref="IHtmlContent"/> containing the <see cref="PreContent"/>.</returns>
        public IHtmlContent GeneratePreContent()
        {
            if (!IsTagNameNullOrWhitespace && SelfClosing)
            {
                return null;
            }

            return _preContent;
        }

        /// <summary>
        /// Generates the <see cref="TagHelperOutput"/>'s body.
        /// </summary>
        /// <returns><c>null</c> if <see cref="TagName"/> is not <c>null</c> or whitespace
        /// and <see cref="SelfClosing"/> is <c>true</c>.
        /// Otherwise, an <see cref="IHtmlContent"/> containing the <see cref="Content"/>.</returns>
        public IHtmlContent GenerateContent()
        {
            if (!IsTagNameNullOrWhitespace && SelfClosing)
            {
                return null;
            }

            return _content;
        }

        /// <summary>
        /// Generates the <see cref="TagHelperOutput"/>'s <see cref="PostContent"/>.
        /// </summary>
        /// <returns><c>null</c> if <see cref="TagName"/> is not <c>null</c> or whitespace
        /// and <see cref="SelfClosing"/> is <c>true</c>.
        /// Otherwise, an <see cref="IHtmlContent"/> containing the <see cref="PostContent"/>.</returns>
        public IHtmlContent GeneratePostContent()
        {
            if (!IsTagNameNullOrWhitespace && SelfClosing)
            {
                return null;
            }

            return _postContent;
        }

        /// <summary>
        /// Generates the <see cref="TagHelperOutput"/>'s end tag.
        /// </summary>
        /// <returns><c>string.Empty</c> if <see cref="TagName"/> is <c>null</c> or whitespace. Otherwise, the
        /// <see cref="string"/> representation of the <see cref="TagHelperOutput"/>'s end tag.</returns>
        public IHtmlContent GenerateEndTag()
        {
            if (SelfClosing || IsTagNameNullOrWhitespace)
            {
                return StringHtmlContent.Empty;
            }

            return StringHtmlContent.FromEncodedText(string.Format(CultureInfo.InvariantCulture, "</{0}>", TagName));
        }

        /// <summary>
        /// Changes <see cref="TagHelperOutput"/> to generate nothing.
        /// </summary>
        /// <remarks>
        /// Sets <see cref="TagName"/> to <c>null</c>, and clears <see cref="PreContent"/>, <see cref="Content"/>,
        /// and <see cref="PostContent"/> to suppress output.
        /// </remarks>
        public void SuppressOutput()
        {
            TagName = null;
            PreContent.Clear();
            Content.Clear();
            PostContent.Clear();
        }
    }
}
