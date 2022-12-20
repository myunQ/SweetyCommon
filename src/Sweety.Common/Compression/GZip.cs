/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 *      使用 GZip 数据格式的压缩/解压缩。
 *      
 * Members Index:
 *      class
 *          GZip
 * * * * * * * * * * * * * * * * * * * * */


namespace Sweety.Common.Compression
{
    using System;
    using System.IO;
    using System.IO.Compression;


    /// <summary>
    /// 使用 GZip 数据格式对数据进行压缩/解压缩
    /// </summary>
    /// <remarks>
    /// 使用 GZip 类压缩大于 4 GB 会抛出异常的文件。
    /// </remarks>
    public class GZip : ICompression
    {
        #region ICompression interface implementation.
        public void Compress(Stream input, Stream output)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (!input.CanRead) throw new ArgumentException(Properties.Localization.the_stream_does_not_support_read_operations, nameof(input));
            if (!output.CanWrite) throw new ArgumentException(Properties.Localization.the_stream_does_not_support_write_operations, nameof(output));
            
            using (GZipStream gzip = new GZipStream(output, CompressionMode.Compress, true))
            {
                input.CopyTo(gzip);
            }
        }

        public void Decompression(Stream input, Stream output)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (!input.CanRead) throw new ArgumentException(Properties.Localization.the_stream_does_not_support_read_operations, nameof(input));
            if (!output.CanWrite) throw new ArgumentException(Properties.Localization.the_stream_does_not_support_write_operations, nameof(output));

            using (GZipStream gzip = new GZipStream(input, CompressionMode.Decompress, true))
            {
                gzip.CopyTo(output);
            }
        }
        #endregion ICompression interface implementation.
    }
}
