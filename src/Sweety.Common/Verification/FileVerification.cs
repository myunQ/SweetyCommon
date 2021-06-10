/* * * * * * * * * * * * * * * * * * * * *
 * Creator: Mingyun Qin
 * E-Mail: myun_18@126.com
 * Description:
 * 
 * Members Index:
 *      class
 *          FileVerification
 *          bool IsImage(Stream stream)
 *          bool IsImage(string name, Stream stream)
 *          IsJPGE(Stream stream)
 *          IsJPGE(FileInfo file)
 *          IsPNG(Stream stream)
 *          IsPNG(FileInfo file)
 *          IsGIF(Stream stream)
 *          IsGIF(FileInfo file)
 *          
 * * * * * * * * * * * * * * * * * * * * */

using System;
using System.IO;

namespace Sweety.Common.Verification
{
    /// <summary>
    /// 文件验证，用于验证文件的真实性、有效性、合法性。
    /// </summary>
    public static class FileVerification
    {
        /// <summary>
        /// 是否是图片流。
        /// </summary>
        /// <param name="stream">图片流。</param>
        /// <returns>如果是图片流则返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool IsImage(Stream stream)
        {
            return FileVerification.IsJPGE(stream)
                || FileVerification.IsPNG(stream)
                || FileVerification.IsGIF(stream);
        }

        /// <summary>
        /// 是否是图片流。
        /// </summary>
        /// <param name="name">包括扩展名的图片文件名。</param>
        /// <param name="stream">图片流。</param>
        /// <returns>如果是扩展名对应的图片流则返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool IsImage(string name, Stream stream)
        {
            switch (Path.GetExtension(name).ToLowerInvariant())
            {
                case ".jpg":
                case ".jpeg":
                    return FileVerification.IsJPGE(stream);
                case ".png":
                    return FileVerification.IsPNG(stream);
                case ".gif":
                    return FileVerification.IsGIF(stream);
                default:
                    throw new NotSupportedException("不支持上传此类文件。");
            }
        }

        /// <summary>
        /// 是否是<c>JPEG</c>格式的图片流。
        /// </summary>
        /// <param name="stream">可读可查找的图片流。</param>
        /// <returns>如果是<c>JPGE</c>图片流则返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool IsJPGE(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            if (!stream.CanRead) throw new NotSupportedException("流不支持读取。");
            if (!stream.CanSeek) throw new NotSupportedException("流不支持查找。");

            long position = stream.Position;

            if (position != 0L)
            {
                stream.Position = 0L;
            }

            int byteValue = stream.ReadByte();

            //JPEG 图片开头的几个字节：0xff, 0xd8, 0xff
            //JPEG 图片结尾的几个字节：0xff 0xd9
            if (byteValue == 0xff)
            {
                byteValue = stream.ReadByte();
                if (byteValue == 0xd8)
                {
                    byteValue = stream.ReadByte();
                    if (byteValue == 0xff)
                    {
                        stream.Position = stream.Length - 2L;
                        byteValue = stream.ReadByte();
                        if (byteValue == 0xff)
                        {
                            byteValue = stream.ReadByte();
                            if (byteValue == 0xd9)
                            {
                                stream.Position = position;
                                return true;
                            }
                        }
                    }
                }
            }

            stream.Position = position;
            return false;
        }

        /// <summary>
        /// 是否是<c>JPEG</c>格式的图片文件。
        /// </summary>
        /// <param name="file">图片文件对象实例。</param>
        /// <returns>如果是<c>JPGE</c>图片文件则返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool IsJPGE(FileInfo file)
        {
            if (file.Name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || file.Name.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return IsJPGE(file.OpenRead());
            }

            return false;
        }

        /// <summary>
        /// 是否是<c>PNG</c>格式的图片流。
        /// </summary>
        /// <param name="stream">可读可查找的图片流。</param>
        /// <returns>如果是<c>PNG</c>图片流则返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool IsPNG(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            if (!stream.CanRead) throw new NotSupportedException("流不支持读取。");
            if (!stream.CanSeek) throw new NotSupportedException("流不支持查找。");

            long position = stream.Position;

            if (position != 0L)
            {
                stream.Position = 0L;
            }

            int byteValue = stream.ReadByte();

            //PNG 图片开头的几个字节：0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A
            //PNG 图片结尾的几个字节：0x00 0x00 0x00 0x00 0x49 0x45 0x4E 0x44 0xAE 0x42 0x60 0x82
            if (byteValue == 0x89)
            {
                byteValue = stream.ReadByte();
                if (byteValue == 0x50)
                {
                    byteValue = stream.ReadByte();
                    if (byteValue == 0x4E)
                    {
                        byteValue = stream.ReadByte();
                        if (byteValue == 0x47)
                        {
                            byteValue = stream.ReadByte();
                            if (byteValue == 0x0D)
                            {
                                byteValue = stream.ReadByte();
                                if (byteValue == 0x0A)
                                {
                                    byteValue = stream.ReadByte();
                                    if (byteValue == 0x1A)
                                    {
                                        byteValue = stream.ReadByte();
                                        if (byteValue == 0x0A)
                                        {
                                            stream.Position = stream.Length - 9L;
                                            byteValue = stream.ReadByte();
                                            if (byteValue == 0x00)
                                            {
                                                byteValue = stream.ReadByte();
                                                if (byteValue == 0x49)
                                                {
                                                    byteValue = stream.ReadByte();
                                                    if (byteValue == 0x45)
                                                    {
                                                        byteValue = stream.ReadByte();
                                                        if (byteValue == 0x4E)
                                                        {
                                                            byteValue = stream.ReadByte();
                                                            if (byteValue == 0x44)
                                                            {
                                                                byteValue = stream.ReadByte();
                                                                if (byteValue == 0xAE)
                                                                {
                                                                    byteValue = stream.ReadByte();
                                                                    if (byteValue == 0x42)
                                                                    {
                                                                        byteValue = stream.ReadByte();
                                                                        if (byteValue == 0x60)
                                                                        {
                                                                            byteValue = stream.ReadByte();
                                                                            if (byteValue == 0x82)
                                                                            {
                                                                                stream.Position = position;
                                                                                return true;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            stream.Position = position;
            return false;
        }

        /// <summary>
        /// 是否是<c>PNG</c>格式的图片文件。
        /// </summary>
        /// <param name="file">图片文件对象实例。</param>
        /// <returns>如果是<c>PNG</c>图片文件则返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool IsPNG(FileInfo file)
        {
            if (file.Name.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                return IsPNG(file.OpenRead());
            }

            return false;
        }

        /// <summary>
        /// 是否是<c>GIF</c>格式的图片流。
        /// </summary>
        /// <param name="stream">可读可查找的图片流。</param>
        /// <returns>如果是<c>GIF</c>图片流则返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool IsGIF(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            if (!stream.CanRead) throw new NotSupportedException("流不支持读取。");
            if (!stream.CanSeek) throw new NotSupportedException("流不支持查找。");

            long position = stream.Position;

            if (position != 0L)
            {
                stream.Position = 0L;
            }

            int byteValue = stream.ReadByte();
            //GIF 图片开头的几个字节：0x47, 0x49, 0x46, 0x38, 0x39, 0x61
            if (byteValue == 0x47)
            {
                byteValue = stream.ReadByte();
                if (byteValue == 0x49)
                {
                    byteValue = stream.ReadByte();
                    if (byteValue == 0x46)
                    {
                        byteValue = stream.ReadByte();
                        if (byteValue == 0x38)
                        {
                            byteValue = stream.ReadByte();
                            if (byteValue == 0x39)
                            {
                                byteValue = stream.ReadByte();
                                if (byteValue == 0x61)
                                {
                                    stream.Position = position;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            stream.Position = position;
            return false;
        }

        /// <summary>
        /// 是否是<c>GIF</c>格式的图片文件。
        /// </summary>
        /// <param name="file">图片文件对象实例。</param>
        /// <returns>如果是<c>GIF</c>图片文件则返回<c>true</c>，否则返回<c>false</c>。</returns>
        public static bool IsGIF(FileInfo file)
        {
            if (file.Name.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
            {
                return IsGIF(file.OpenRead());
            }

            return false;
        }
    }
}
