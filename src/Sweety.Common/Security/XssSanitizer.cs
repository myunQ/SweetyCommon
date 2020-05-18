/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  基于白名单过滤可能造成跨站脚本攻击的元素。
 * 
 * Members Index:
 *      class
 *          XssSanitizer
 *              
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Security
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;

    using Sweety.Common.Verification;


    /// <summary>
    /// 清除可能造成跨站脚本攻击的 <c>HTML</c> 内容。
    /// </summary>
    public class XssSanitizer : XssSanitizerBase, IXssSanitizer
    {
        static IXssSanitizer __default_instance = null;

        /// <summary>
        /// 默认实例。
        /// </summary>
        public static IXssSanitizer Default
        {
            get
            {
                if (__default_instance == null)
                {
                    __default_instance = new XssSanitizer();
                }
                return __default_instance;
            }
        }

        public XssSanitizer() : base() { }

        public XssSanitizer(IEnumerable<string> allowedTags = null, IEnumerable<string> allowedAttributes = null, IEnumerable<string> allowedCssProperties = null)
            : base(allowedTags, allowedAttributes, allowedCssProperties) { }


        /// <summary>
        /// 清理存在跨站脚本攻击的内容。
        /// </summary>
        /// <param name="html">需要清理的 <c>HTML</c>。</param>
        /// <param name="sanitizedHtml">已清理的 <c>HTML</c>。</param>
        /// <returns>有内容被清理则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public override bool Sanitize(string html, out string sanitizedHtml)
        {
            if (String.IsNullOrEmpty(html))
            {
                sanitizedHtml = html;
                return false;
            }

            sanitizedHtml = HtmlParser(html);
            return sanitizedHtml != html;
        }

        /// <summary>
        /// 解析并清除有危害的 <c>HTML</c> 文档内容。
        /// </summary>
        /// <param name="html"><c>HTML</c> 文档内容。</param>
        /// <returns>安全的 <paramref name="html"/> 内容。</returns>
        private string HtmlParser(string html)
        {
            int startIndex = 0;
            StringBuilder result = null;
            for (int i = 0, len = html.Length; i < len; i++)
            {
                if (html[i] == '<' && len > i + 1)
                {
                    char nextChar = html[i + 1];
                    //if (CharVerification.IsSBCLetter(nextChar) || (nextChar == '/' && len > i + 2 && CharVerification.IsSBCLetter(html[i+2])))
                    if (CharVerification.IsSBCLetter(nextChar) || nextChar == '/')
                    {
                        if (result == null) result = new StringBuilder(len);
                        if (i > startIndex) result.Append(html, startIndex, i - startIndex);

                        if (nextChar == '/' && len > i + 2 && !CharVerification.IsSBCLetter(html[i + 2]))
                        {
                            result.Append("<&#47;");
                            i++;
                        }
                        else
                        {
                            TagParser(result, html, len, ref i);
                        }
                        startIndex = i + 1;
                    }
                    else if (nextChar == '!')
                    {
                        if (result == null) result = new StringBuilder(len);

                        if (i > startIndex) result.Append(html, startIndex, i - startIndex);

                        int oIndex = i;
                        if ((i < len - 3) && (html[i + 2] == '-') && (html[i + 3] == '-'))
                        {
                            i += 4; //定位到“<!--”之后的字符。
                            CommentParser(html, len, ref i);
                        }
                        else
                        {
                            i += 2; //定位到“<!”之后的字符。
                            DeclarationParser(html, len, ref i); // <!DOCTYPE ...>标签处理。
                        }

                        if (i >= len)
                        {
                            result.Append("&lt;").Append(html, oIndex + 1, len - oIndex - 1);
                        }

                        startIndex = i + 1;
                    }
                    else if (result != null)
                    {
                        startIndex = i + 1;
                        result.Append(html[i]);
                    }
                }
                else if (result != null)
                {
                    startIndex = i + 1;
                    result.Append(html[i]);
                }
            }

            return (result == null) ? html : result.ToString();
        }

        /// <summary>
        /// 对一个标签进行解析。
        /// </summary>
        /// <param name="result">接收解析后的 <c>HTML</c> 文档内容的容器。</param>
        /// <param name="html"><c>HTML</c> 文档内容。</param>
        /// <param name="len"><paramref name="html"/> 的字符数。</param>
        /// <param name="i">标签起始字符“&lt;”在 <paramref name="html"/> 中的索引。</param>
        private void TagParser(StringBuilder result, string html, int len, ref int i)
        {
            int tnMaxIndex = 16;
            char[] tagName = new char[tnMaxIndex--]; //把 tnMaxIndex 减去 1 使其成为 tagName 的最大索引。
            int tnIndex = 0;
            int oIndex = i; //在方法的其它地方不要改变 oIndex 的值。

            while (++i < len)
            {
                if (html[i] == '/') continue;

                //if (CharVerification.IsSBCLetter(html[i]) || CharVerification.IsSBCDigit(html[i]))
                if (html[i] != '<' && html[i] != '>' && !CharVerification.IsBlank(html[i]))
                {
                    tagName[tnIndex++] = html[i];
                    if (tnIndex > tnMaxIndex)
                    {
                        //不允许的标签，因为标签名超长了，这肯定不是一个有效的 HTML 标签。
                        result.Append("&lt;");
                        i = oIndex;
                        return;
                    }
                }
                else
                {
                    string tn = new String(tagName, 0, tnIndex);
                    if (!AllowedTags.Contains(tn))
                    {
                        //不允许的标签
                        if (html[oIndex + 1] != '/' && "script".Equals(tn, StringComparison.OrdinalIgnoreCase) || "style".Equals(tn, StringComparison.OrdinalIgnoreCase))
                        {
                            SkinClosedTagEndChar(html, tn, len, ref i);
                        }
                        else
                        {
                            if (!SkinTagEndChar(html, len, ref i))
                            {
                                if (i >= len)
                                {
                                    for (int x = oIndex; x < len; x++)
                                    {
                                        if (html[x] == '<')
                                        {
                                            result.Append("&lt;");
                                        }
                                        else if (html[x] == '>')
                                        {
                                            result.Append("&gt;");
                                        }
                                        else
                                        {
                                            result.Append(html[x]);
                                        }
                                    }
                                }
                                else
                                {
                                    result.Append("&lt;").Append(html, oIndex + 1, i - oIndex);
                                }
                            }
                        }
                        return;
                    }
                    else
                    {
                        result.Append(html, oIndex, i - oIndex + 1);

                        if (html[i] == '>') return;
                        break;
                    }
                }
            }

            //以下处理标签的属性
            while (++i < len)
            {
                if (html[i] != '/' && html[i] != '>' && !CharVerification.IsBlank(html[i]))
                {
                    AttributeParser(result, html, len, ref i);
                    if (i >= len) break;
                }

                if (html[i] == '>')
                {
                    while (result.Length > 0 && CharVerification.IsBlank(result[result.Length - 1])) result.Length--;

                    result.Append('>');
                    return;
                }
                else if (html[i] == '<')
                {
                    result.Replace("<", "&lt;", oIndex, 1);
                    i--;
                    return;
                }
                else
                {
                    if (html[i] == ' ' && result[result.Length - 1] == ' ') continue;

                    result.Append(html[i]);
                }
            }

            //走道这的都是有“< + 英文字母”且后面没有“>”的，所以需要将“<”转成“&lt;”。
            if (result.Length > oIndex) result.Length = oIndex;
            result.Append("&lt;").Append(html, oIndex + 1, len - oIndex - 1);
        }

        /// <summary>
        /// 对标签属性进行解析。
        /// </summary>
        /// <param name="result"></param>
        /// <param name="html"></param>
        /// <param name="len"></param>
        /// <param name="i">属性名的第一个字符在<paramref name="html"/>中的索引。</param>
        private void AttributeParser(StringBuilder result, string html, int len, ref int i)
        {
            int anMaxIndex = 16;
            char[] attrName = new char[anMaxIndex--]; //把 anMaxIndex 减去 1 使其成为 anMaxIndex 的最大索引。
            int anIndex = 0;
            int oIndex = i;

            for (; i < len; i++)
            {
                if (!CharVerification.IsBlank(html[i]) && html[i] != '=' && html[i] != '>')
                {
                    if (i + 1 == len)
                    {
                        i++;
                        result.Append(html, oIndex, i - oIndex);
                        return;
                    }

                    attrName[anIndex++] = html[i];
                    if (anIndex > anMaxIndex)
                    {
                        //不允许的属性
                        //while (++i < len && !CharVerification.IsBlank(html[i]) && html[i] != '>')
                        //{
                        //    if (html[i] == '=')
                        //    {
                        //        GetAttributeValue(html, len, ref i);
                        //        break;
                        //    }
                        //}
                        GetAttributeValue(html, len, ref i);
                        return;
                    }
                }
                else
                {
                    string an = new String(attrName, 0, anIndex);
                    if (!AllowedAttributes.Contains(an))
                    {
                        //不允许的属性
                        GetAttributeValue(html, len, ref i);
                        return;
                    }
                    else
                    {
                        result.Append(html, oIndex, i - oIndex);
                        do
                        {
                            if (html[i] == '=')
                            {
                                result.Append('=');
                                break;
                            }

                            if (!CharVerification.IsBlank(html[i]))
                            {
                                //没有遇到赋值运算符“=”就遇到非空白符“\r \n \t (空格)”表示属性没有值。退出属性分析方法。例如：<input disabled />
                                return;
                            }
                        } while (++i < len);

                        if (i >= len || ++i >= len) return;

                        string attrValue = GetAttributeValue(html, len, ref i);

                        if ("style".Equals(an, StringComparison.OrdinalIgnoreCase))
                        {
                            result.Append(FilterStyleAttributeValue(attrValue));
                        }
                        else if ("href".Equals(an, StringComparison.OrdinalIgnoreCase))
                        {
                            string securityValue = HrefAttributeValueHandler(attrValue);
                            if (securityValue == "\"\"")
                            {
                                result.Length -= 6; //6 = " href=".Length
                            }
                            else
                            {
                                result.Append(securityValue);
                            }
                        }
                        else
                        {
                            result.Append(attrValue);
                        }
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 获取 <c>HTML</c> 标签的属性值（即：标签里“=”右边的内容）。
        /// </summary>
        /// <param name="html"><c>HTML</c> 文档内容。</param>
        /// <param name="len"><paramref name="html"/> 的字符数。</param>
        /// <param name="i">“=”右边的第一个字符在 <paramref name="html"/> 中的索引。</param>
        /// <returns>用双引号括住的属性值。</returns>
        private string GetAttributeValue(string html, int len, ref int i)
        {
            int oIndex = i;
            bool s_quot = false; //true表示字符在单引号内。
            bool d_quot = false; //true表示字符在双引号内。
            bool enteredQuotes = false; //true表示遇到过引号字符。
            bool noQuotes = false; //true表示属性值没有用引号括住。例如：<img name=userAvatar src=... />
            do
            {
                if (html[i] == '>')
                {
                    if (!(s_quot || d_quot)) break;
                }
                else if (html[i] == '"')
                {
                    if (!(s_quot || noQuotes))
                    {
                        d_quot = !d_quot;
                        if (!enteredQuotes) enteredQuotes = true;
                    }
                }
                else if (html[i] == '\'')
                {
                    if (!(d_quot || noQuotes))
                    {
                        s_quot = !s_quot;
                        if (!enteredQuotes) enteredQuotes = true;
                    }
                }
                else
                {
                    if (enteredQuotes)
                    {
                        if (!(s_quot || d_quot))
                        {
                            //已经到了闭合的双引号或单引号，属性值结束。例如：<img src="..." />
                            break;
                        }
                    }
                    else if (!CharVerification.IsBlank(html[i]))
                    {
                        if (!noQuotes) noQuotes = true;
                    }
                    else if (noQuotes)
                    {
                        //属性值没有引号括住的情况下，遇到空白字符，属性值结束。例如：<img name=userAvater src="..." />
                        break;
                    }
                    else
                    {
                        oIndex++; //跳过“=”与属性值之间的空白符。
                    }
                }

            } while (++i < len);

            if (i > oIndex)
            {
                if (noQuotes)
                {
                    return "\"" + html.Substring(oIndex, i - oIndex).Replace("\"", "&quot;") + "\"";
                }
                else if (html[oIndex] == '\'')
                {
                    return "\"" + html.Substring(oIndex, i - oIndex).Trim('\'').Replace("\"", "&quot;") + "\"";
                }

                return html.Substring(oIndex, i - oIndex);
            }
            return "\"\"";
        }

        /// <summary>
        /// 过滤不允许使用的样式表属性。
        /// </summary>
        /// <param name="attrValue"><c>HTML</c> 标签中 <c>style</c> 属性的值。</param>
        /// <returns>用双引号括住的允许使用的样式表属性。</returns>
        private string FilterStyleAttributeValue(string attrValue)
        {
            if (String.IsNullOrEmpty(attrValue)) return "\"\"";

            StringBuilder result = new StringBuilder(attrValue.Length * 128);
            result.Append('\"');

            if (attrValue[0] == '\"' || attrValue[0] == '\'')
            {
                attrValue = HttpUtility.HtmlDecode(attrValue.Trim(attrValue[0])).Trim();
            }
            else
            {
                attrValue = HttpUtility.HtmlDecode(attrValue).Trim();
            }

            foreach (var item in attrValue.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var nv = item.Split(':');
                if (nv.Length != 2 || !AllowedCssProperties.Contains(nv[0].Trim())) continue;

                result.Append(item.Replace("\"", "&quot;"));
                result.Append(';');
            }

            return result.Append('\"').ToString();
        }

        /// <summary>
        /// 对 <c>href</c> 属性的值进行安全隐患排除处理。过滤掉 <c>href="javascript:..."</c> 这样的危险内容。
        /// </summary>
        /// <param name="attrValue"><c>HTML</c> 标签中 <c>href</c> 属性的值。</param>
        /// <returns>用双引号括住的安全的属性值。</returns>
        private string HrefAttributeValueHandler(string attrValue)
        {
            if (String.IsNullOrEmpty(attrValue)) return "\"\"";

            if (attrValue[0] == '\"' || attrValue[0] == '\'')
            {
                attrValue = HttpUtility.HtmlDecode(attrValue.Trim(attrValue[0])).Trim();
            }
            else
            {
                attrValue = HttpUtility.HtmlDecode(attrValue).Trim();
            }

            char[] js = new char[] { 'j', 'a', 'v', 'a', 's', 'c', 'r', 'i', 'p', 't', ':' };
            int i = 0;
            int j = attrValue.Length;
            int x = 0;
            int y = js.Length;
            if (j >= y)
            {
                while (i < j && x < y)
                {
                    if (attrValue[i] == js[x] || Char.ToLower(attrValue[i]) == js[x])
                    {
                        i++;
                        x++;
                    }
                    else if (attrValue[i] == '\r' || attrValue[i] == '\n')
                    {
                        if (i + 1 < j && attrValue[i + 1] == '\n')
                        {
                            i += 2;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }

            return (x < y) ? "\"" + attrValue.Replace("\"", "&quot;") + "\"" : "\"\"";
        }

        /// <summary>
        /// <c>HTML</c> 注释标签解析处理。
        /// </summary>
        /// <param name="html"><c>HTML</c> 文档内容。</param>
        /// <param name="len"><paramref name="html"/> 的字符数。</param>
        /// <param name="i">“&lt;!--”右边的第一个字符在 <paramref name="html"/> 中的索引。</param>
        private void CommentParser(string html, int len, ref int i)
        {
            len -= 2;
            for (; i < len; i++)
            {
                if (html[i] == '-' && html[i + 1] == '-' && html[i + 2] == '>')
                {
                    i += 2; //定位到“-->”的“>”字符。
                    return;
                }
            }

            i = html.Length;
        }

        /// <summary>
        /// <c>HTML</c> 文档声明标签解析处理。
        /// </summary>
        /// <param name="html"><c>HTML</c> 文档内容。</param>
        /// <param name="len"><paramref name="html"/> 的字符数。</param>
        /// <param name="i">“&lt;!”右边的第一个字符在 <paramref name="html"/> 中的索引。</param>
        private void DeclarationParser(string html, int len, ref int i)
        {
            for (; i < len; i++)
            {
                //在文档声明标签里即使“>”出现在引号中也会当作是标签的结束符号，因此这里不考虑“>”字符是否在引号中。
                if (html[i] == '>')
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 将 <paramref name="i"/> 定位到当前标签的“&gt;”字符位置。
        /// </summary>
        /// <param name="html"><c>HTML</c> 文档内容。</param>
        /// <param name="len"><paramref name="html"/> 的字符数。</param>
        /// <param name="i">标签名或标签名右边的第一个字符在 <paramref name="html"/> 中的索引。</param>
        /// <returns>如果遇到了标签结束字符“&gt;”则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        private bool SkinTagEndChar(string html, int len, ref int i)
        {
            bool s_quot = false; //true表示字符在单引号内。
            bool d_quot = false; //true表示字符在双引号内。
            bool blank = false; //true表示出现过空白符。
            bool assignmentOperator = false; //true表示参数i正处于属性值中。
            bool enteredQuotes = false; //true表示属性值是使用单引号或双引号括住的。

            do
            {
                if (CharVerification.IsBlank(html[i]))
                {
                    if (blank == false) blank = true;
                }
                else if (html[i] == '>')
                {
                    if (!(s_quot || d_quot)) return true;
                }
                else if (html[i] == '<')
                {
                    if (!(s_quot || d_quot))
                    {
                        i--;
                        break;
                    }
                }
                else if (html[i] == '"')
                {
                    if (assignmentOperator && !s_quot && enteredQuotes)
                    {
                        d_quot = false;
                        enteredQuotes = false;
                        assignmentOperator = false;
                    }
                }
                else if (html[i] == '\'')
                {
                    if (assignmentOperator && !d_quot && enteredQuotes)
                    {
                        s_quot = false;
                        enteredQuotes = false;
                        assignmentOperator = false;
                    }
                }
                else if (blank)
                {
                    if (assignmentOperator == false && html[i] == '=')
                    {
                        assignmentOperator = true;
                        while (++i < len) if (!CharVerification.IsBlank(html[i])) break;

                        if (html[i] == '<' || html[i] == '>')
                        {
                            i--;
                            continue;
                        }

                        if (html[i] == '"')
                        {
                            enteredQuotes = true;
                            d_quot = true;
                        }
                        else if (html[i] == '\'')
                        {
                            enteredQuotes = true;
                            s_quot = true;
                        }
                    }
                }
            } while (++i < len);

            return false;
        }

        /// <summary>
        /// 将 <paramref name="i"/> 定位到当前标签的闭合标签的“&gt;”字符位置。
        /// </summary>
        /// <param name="html"><c>HTML</c> 文档内容。</param>
        /// <param name="len"><paramref name="html"/> 的字符数。</param>
        /// <param name="i">标签名或标签名右边的第一个字符在 <paramref name="html"/> 中的索引。</param>
        private void SkinClosedTagEndChar(string html, string tagName, int len, ref int i)
        {
            bool s_quot = false; //true表示字符在单引号内。
            bool d_quot = false; //true表示字符在双引号内。
            do
            {
                if (html[i] == '/' && !(s_quot || d_quot))
                {
                    if (i + 1 < len && html[i + 1] == '>')
                    {
                        //遇到“/>”的结束符，例如：<hr />、<br />、<img />
                        if ("img".Equals(tagName, StringComparison.OrdinalIgnoreCase) ||
                            "br".Equals(tagName, StringComparison.OrdinalIgnoreCase) ||
                            "hr".Equals(tagName, StringComparison.OrdinalIgnoreCase))
                        {
                            i++;
                            return;
                        }
                    }
                }
                else if (html[i] == '<' && !(s_quot || d_quot))
                {
                    if (i + 1 < len && html[i + 1] == '/')
                    {
                        bool isClosedTag = true;

                        i += 2;
                        for (int x = 0, y = tagName.Length; x < y && i < len; i++, x++)
                        {
                            if (html[i] != tagName[x])
                            {
                                isClosedTag = false;
                                break;
                            }
                        }

                        if (isClosedTag)
                        {
                            while (i < len)
                            {
                                if (html[i] == '>')
                                {
                                    return;
                                }
                                else if (!CharVerification.IsBlank(html[i]))
                                {
                                    break;
                                }
                                i++;
                            }
                        }
                    }
                    else
                    {
                        //例如：<div>外层<div>内层</div>2外层2</div> 的HTML结构不会将内外层的结束标签搞混了。
                        int x = 0;
                        int y = tagName.Length;
                        while (x < y && ++i < len)
                        {
                            if (html[i] != tagName[x])
                            {
                                break;
                            }
                            x++;
                        }

                        if (x == y && i < len) SkinClosedTagEndChar(html, tagName, len, ref i);
                    }
                }
                else if (html[i] == '"' && !s_quot)
                {
                    d_quot = !d_quot;
                }
                else if (html[i] == '\'' && !d_quot)
                {
                    s_quot = !s_quot;
                }
            } while (++i < len);
        }
    }
}
