/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *  基于白名单清除可能造成跨站脚本攻击的可疑代码的接口。
 * 
 * Members Index:
 *      class
 *          IXssSanitizer
 *              
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.Security
{
    using System.Collections.Generic;


    /// <summary>
    /// 基于白名单清除可能造成跨站脚本攻击的可疑代码的接口。
    /// </summary>
    public interface IXssSanitizer
    {
        /// <summary>
        /// 获取允许的标签集合。
        /// </summary>
        ISet<string> AllowedTags { get; }
        /// <summary>
        /// 获取允许的HTML标签属性集合。
        /// </summary>
        ISet<string> AllowedAttributes { get; }
        /// <summary>
        /// 获取允许的样式表属性集合。
        /// </summary>
        ISet<string> AllowedCssProperties { get; }

        /// <summary>
        /// 清理存在跨站脚本攻击的内容。
        /// </summary>
        /// <param name="html">需要做清理的 <c>HTML</c> 文本。</param>
        /// <param name="sanitizedHtml">清理过的 <c>HTML</c> 文本。</param>
        /// <returns>有内容被清理则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        bool Sanitize(string html, out string sanitizedHtml);
    }
}
