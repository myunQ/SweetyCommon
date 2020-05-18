/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  基于白名单的方式清理跨站脚本攻击的可疑代码的基类。
 * 
 * Members Index:
 *      class
 *          XssSanitizerBase
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 基于白名单的方式清理跨站脚本攻击的基类。
    /// </summary>
    public abstract class XssSanitizerBase : IXssSanitizer
    {
        /// <summary>
        /// 默认的 <c>HTML</c> 标签白名单。
        /// </summary>
        internal static readonly ISet<string> DEFAULT_ALLOWD_TAGS;

        /// <summary>
        /// 默认的 <c>HTML</c> 标签属性白名单。
        /// </summary>
        internal static ISet<string> DEFAULT_ALLOWED_ATTRIBUTES;

        /// <summary>
        /// 默认的样式属性白名单。
        /// </summary>
        internal static ISet<string> DEFAULT_ALLOWED_CSS_PROPERTIES;

        /// <summary>
        /// 静态构造函数初始化默认白名单。
        /// </summary>
        static XssSanitizerBase()
        {
            InternalHashSet set = new InternalHashSet()
            {
                // https://developer.mozilla.org/en/docs/Web/Guide/HTML/HTML5/HTML5_element_list
                "h1", "h2", "h3", "h4", "h5", "h6",
                "p", "div", "pre",
                "ul", "ol", "li", "dl", "dt", "dd",
                "span", "font", "bdo",
                "i", "b", "u", "s", "del", "sub", "sup", "big", "small", "ins", "strike", "tt", "marquee",
                "em", "strong", "dfn", "code", "samp", "kbd", "var", "cite",
                "table", "caption", "thead", "tbody", "tfoot", "th", "tr", "td", "col", "colgroup",
                "abbr", "acronym", "address",
                "q", "blockquote",
                "fieldset", "legend",
                "a", "img", "map", "area",
                "br", "hr",
            
                // HTML5
                // Sections
                "section", "nav", "article", "aside", "header", "footer", "main",
                // Grouping content
                "figure", "figcaption",
                // Text-level semantics
                "time", "mark", "ruby", "rt", "rp", "bdi", "wbr",
                // Interactive elements
                "details", "summary",
                //Media
                "canvas", "audio", "video", "source", "track"
            };
            set.IsDefault = true;
            DEFAULT_ALLOWD_TAGS = set;


            set = new InternalHashSet()
            {
                //https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes
                "accesskey",
                "align", "alt", "bgcolor", "border", "cellpadding",
                "cellspacing", "char", "charoff", "cite", /* "class", */
                "colspan", "color", "coords", "controls", "datetime",
                "dir", "headers", "height",
                "href", "hreflang", /* "id", */ "ismap", "label", "lang",
                "media", "name",
                "nohref", "noshade", "nowrap", "rel", "rev",
                "rowspan", "rules", "scope", "shape",
                "span", "src", "start", "style", "summary", "tabindex", "target", "title",
                "type", "usemap", "valign", "vspace", "width",
                // HTML5
                "high", // <meter>
                "low", // <meter>
                "max", // <input>, <meter>, <progress>
                "min", // <input>, <meter>
                "open", // <details>
                "optimum", // <meter>
                "pubdate", // <time>
                "reversed", // <ol>
                "spellcheck", // Global attribute
                "contenteditable", // Global attribute
                "draggable", // Global attribute
                "dropzone" // Global attribute
            };
            set.IsDefault = true;
            DEFAULT_ALLOWED_ATTRIBUTES = set;

            set = new InternalHashSet()
            {
                "background",
                "background-attachment",
                "background-clip",
                "background-color",
                "background-image",
                "background-origin",
                "background-position",
                "background-repeat",
                "background-size",
                "border",
                "border-bottom",
                "border-bottom-color",
                "border-bottom-left-radius",
                "border-bottom-right-radius",
                "border-bottom-style",
                "border-bottom-width",
                "border-collapse",
                "border-color",
                "border-image",
                "border-image-outset",
                "border-image-repeat",
                "border-image-slice",
                "border-image-source",
                "border-image-width",
                "border-left",
                "border-left-color",
                "border-left-style",
                "border-left-width",
                "border-radius",
                "border-right",
                "border-right-color",
                "border-right-style",
                "border-right-width",
                "border-spacing",
                "border-style",
                "border-top",
                "border-top-color",
                "border-top-left-radius",
                "border-top-right-radius",
                "border-top-style",
                "border-top-width",
                "border-width",
                "caption-side",
                "clear",
                "clip",
                "color",
                "content",
                "counter-increment",
                "counter-reset",
                "cursor",
                "direction",
                "display",
                "empty-cells",
                "float",
                "font",
                "font-family",
                "font-feature-settings",
                "font-kerning",
                "font-language-override",
                "font-size",
                "font-size-adjust",
                "font-stretch",
                "font-style",
                "font-synthesis",
                "font-variant",
                "font-variant-alternates",
                "font-variant-caps",
                "font-variant-east-asian",
                "font-variant-ligatures",
                "font-variant-numeric",
                "font-variant-position",
                "font-weight",
                "height",
                "letter-spacing",
                "line-height",
                "list-style",
                "list-style-image",
                "list-style-position",
                "list-style-type",
                "margin",
                "margin-bottom",
                "margin-left",
                "margin-right",
                "margin-top",
                "max-height",
                "max-width",
                "min-height",
                "min-width",
                "opacity",
                "orphans",
                "outline",
                "outline-color",
                "outline-offset",
                "outline-style",
                "outline-width",
                "overflow",
                "overflow-wrap",
                "overflow-x",
                "overflow-y",
                "padding",
                "padding-bottom",
                "padding-left",
                "padding-right",
                "padding-top",
                "page-break-after",
                "page-break-before",
                "page-break-inside",
                "quotes",
                "table-layout",
                "text-align",
                "text-decoration",
                "text-decoration-color",
                "text-decoration-line",
                "text-decoration-skip",
                "text-decoration-style",
                "text-indent",
                "text-transform",
                "unicode-bidi",
                "vertical-align",
                "visibility",
                "white-space",
                "widows",
                "width",
                "word-spacing"
            };
            set.IsDefault = true;
            DEFAULT_ALLOWED_CSS_PROPERTIES = set;
        }

        /// <summary>
        /// 使用默认的白名单初始化实例
        /// </summary>
        public XssSanitizerBase()
        {
            this.AllowedTags = DEFAULT_ALLOWD_TAGS;
            this.AllowedAttributes = DEFAULT_ALLOWED_ATTRIBUTES;
            this.AllowedCssProperties = DEFAULT_ALLOWED_CSS_PROPERTIES;
        }
        /// <summary>
        /// 使用默认的或指定的白名单初始化实例。
        /// </summary>
        /// <param name="allowedTags"><c>HTML</c> 标签白名单。</param>
        /// <param name="allowedAttributes"><c>HTML</c> 标签属性白名单。</param>
        /// <param name="allowedCssProperties">样式属性白名单。</param>
        public XssSanitizerBase(IEnumerable<string> allowedTags = null, IEnumerable<string> allowedAttributes = null, IEnumerable<string> allowedCssProperties = null)
        {
            if (allowedTags != null && allowedTags.Any())
            {
                this.AllowedTags = new InternalHashSet(allowedTags);
            }
            else
            {
                this.AllowedTags = new InternalHashSet(DEFAULT_ALLOWD_TAGS);
            }

            if (allowedAttributes != null && allowedAttributes.Any())
            {
                this.AllowedAttributes = new InternalHashSet(allowedAttributes);
            }
            else
            {
                this.AllowedAttributes = new InternalHashSet(DEFAULT_ALLOWED_ATTRIBUTES);
            }

            if (allowedCssProperties != null && allowedCssProperties.Any())
            {
                this.AllowedCssProperties = new InternalHashSet(allowedCssProperties);
            }
            else
            {
                this.AllowedCssProperties = new InternalHashSet(DEFAULT_ALLOWED_CSS_PROPERTIES);
            }
        }

        #region IXssSanitizer interface implementation.
        public ISet<string> AllowedTags { get; private set; }

        public ISet<string> AllowedAttributes { get; private set; }

        public ISet<string> AllowedCssProperties { get; private set; }

        public abstract bool Sanitize(string html, out string sanitizedHtml);
        #endregion IXssSanitizer interface implementation.
    }
}
