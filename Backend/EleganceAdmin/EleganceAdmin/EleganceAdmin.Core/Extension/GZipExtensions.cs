using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace EleganceAdmin.Core.Extension
{
    /// <summary>
    /// GZip压缩扩展
    /// </summary>
    /// <remarks>
    /// 编码：UTF-8
    /// 压缩：GZip Base64
    /// </remarks>
    public static class GZipExtensions
    {
        private static byte[] GetBytes(this string text) => Encoding.UTF8.GetBytes(text);
        private static string GetString(this byte[] bytes) => Encoding.UTF8.GetString(bytes);
        private static byte[] GetBase64Bytes(this string text) => Convert.FromBase64String(text);
        private static string GetBase64String(this byte[] bytes) => Convert.ToBase64String(bytes);

        /// <summary>GZip压缩</summary>
        public static string Compress(this string text) => text.GetBytes().Compress().GetBase64String();

        /// <summary>GZip压缩</summary>
        public static byte[] Compress(this byte[] bytes)
        {
            using var ms = new MemoryStream();
            var zip = new GZipStream(ms, CompressionMode.Compress);
            zip.Write(bytes, 0, bytes.Length);
            zip.Close();
            return ms.ToArray();
        }

        /// <summary>GZip解压缩</summary>
        public static string Decompress(this string base64) => base64.GetBase64Bytes().Decompress().GetString();

        /// <summary>GZip解压缩</summary>
        public static byte[] Decompress(this byte[] bytes)
        {
            using var ms = new MemoryStream(bytes);
            using var msTemp = new MemoryStream();
            var zip = new GZipStream(ms, CompressionMode.Decompress);
            zip.CopyTo(msTemp);
            zip.Close();
            return msTemp.ToArray();
        }
    }
}
