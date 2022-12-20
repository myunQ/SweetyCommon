/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      定义压缩/解压的方法
 *      
 * Members Index:
 *      interface
 *          ICompression
 * * * * * * * * * * * * * * * * * * * * */

namespace Sweety.Common.Compression
{
    using System;
    using System.IO;


    /// <summary>
    /// 压缩/解压功能接口
    /// </summary>
    public interface ICompression
    {
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="input">要压缩的流</param>
        /// <param name="output">已压缩的流</param>
        void Compress(Stream input, Stream output);

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="input">已压缩的流</param>
        /// <param name="output">接压缩后的流</param>
        void Decompression(Stream input, Stream output);
    }
}
