/* * * * * * * * * * * * * * * * * * * * *
 * Creator:myun
 * E-mail:myun168@gmail.com
 * CreateDate:2014/1/22 14:10:21
 * Description:
 * 
 * Members Index:
 *      class
 *          TextFilter
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.Filter
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;


    /// <summary>
    /// 文本过滤器
    /// </summary>
    public static class TextFilter
    {
        /// <summary>
        /// 过滤文本中的所有 XML 标签
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <returns>过滤掉 XML 标签后的文本内容</returns>
        public static string FilterXmlTags(string text)
        {
            if (String.IsNullOrEmpty(text)) return text;

            return Regex.Replace(text, "<[^>]+>", String.Empty, RegexOptions.Compiled);
        }

        /// <summary>
        /// 过滤文中的指定标签
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="tagName">标签名</param>
        /// <returns>过滤掉 <paramref name="tagName"/> 标签后的文本内容</returns>
        public static string FilterXmlTags(string text, string tagName)
        {
            return FilterXmlTags(text, tagName, String.Empty);
        }

        /// <summary>
        /// 过滤文中的指定标签
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="tagName">标签名</param>
        /// <param name="replace">取代被过滤掉的标签的文本</param>
        /// <returns>过滤掉 <paramref name="tagName"/> 标签后的文本内容</returns>
        public static string FilterXmlTags(string text, string tagName, string replace)
        {
            if (String.IsNullOrEmpty(text)) return text;

            return Regex.Replace(text, "</? *" + tagName + "[^>]*>", replace, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 过滤文本中的所有 HTML 标签
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <returns>过滤掉 HTML 标签后的文本内容</returns>
        public static string FilterHtmlTags(string text)
        {
            if (String.IsNullOrEmpty(text)) return text;

            //text = Regex.Replace(text, "<audio[^>]*>(\\s|.)*?</audio>", String.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            //text = Regex.Replace(text, "<noframes[^>]*>(\\s|.)*?</noframes>", String.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            //text = Regex.Replace(text, "<noscript[^>]*>(\\s|.)*?</noscript>", String.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            //text = Regex.Replace(text, "<script[^>]*>(\\s|.)*?</script>", String.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            //text = Regex.Replace(text, "<style[^>]*>(\\s|.)*?</style>", String.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            //text = Regex.Replace(text, "<video[^>]*>(\\s|.)*?</video>", String.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            //return Regex.Replace(text, "<[^>]+>", String.Empty);

            return Regex.Replace(text, "<audio[^>]*>(\\s|.)*?</audio>|<noframes[^>]*>(\\s|.)*?</noframes>|<noscript[^>]*>(\\s|.)*?</noscript>|<script[^>]*>(\\s|.)*?</script>|<style[^>]*>(\\s|.)*?</style>|<video[^>]*>(\\s|.)*?</video>|<[^>]+>", String.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        /// 过滤文中的指定标签
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="tagName">HTML 标签名</param>
        /// <returns>过滤掉 HTML 标签后的文本内容</returns>
        public static string FilterHtmlTags(string text, string tagName)
        {
            return FilterHtmlTags(text, tagName, String.Empty);
        }

        /// <summary>
        /// 过滤文中的指定标签
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="tagName">HTML 标签名</param>
        /// <param name="replace">取代被过滤掉的HTML标签的文本</param>
        /// <returns>过滤掉 HTML 标签后的文本内容</returns>
        public static string FilterHtmlTags(string text, string tagName, string replace)
        {
            if (String.IsNullOrEmpty(text)) return text;

            if ("audio".Equals(tagName, StringComparison.OrdinalIgnoreCase)
                || "video".Equals(tagName, StringComparison.OrdinalIgnoreCase)
                || "script".Equals(tagName, StringComparison.OrdinalIgnoreCase)
                || "style".Equals(tagName, StringComparison.OrdinalIgnoreCase)
                || "noframes".Equals(tagName, StringComparison.OrdinalIgnoreCase)
                || "noscript".Equals(tagName, StringComparison.OrdinalIgnoreCase))
            {
                return Regex.Replace(text, "<" + tagName + "[^>]*>(\\s|.)*?</" + tagName + ">", replace, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
            else
            {
                return Regex.Replace(text, "</? *" + tagName + "[^>]*>", replace, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
        }



        /// <summary>
        /// 过滤常见标点符号
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <returns>过滤掉标点符号的文本内容</returns>
        public static string FilterPunctuation(string text)
        {
            if (String.IsNullOrEmpty(text)) return text;

            return Regex.Replace(text, @"[\.\^\$\+\*\{\}\[\]\(\)\?\|\\" + " ,<>/:;'\"`~!@#%&-_=" + "，。《》〈〉、？：；‘’“”〝〞［］【】〖〗『』｛｝～·！＠＃﹩￥％＆﹡×…（）—]", String.Empty);
        }
    }
}
