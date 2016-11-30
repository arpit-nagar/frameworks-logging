using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Tavisca.Frameworks.Logging.Infrastructure;

namespace Tavisca.Frameworks.Logging.Compression
{
    public class DeflateCompressionProvider : ICompressionProvider
    {
        public string Compress(string dataToCompress)
        {
            var compressed = Compress(Encoding.Default.GetBytes(dataToCompress));

            return Encoding.Default.GetString(compressed);
        }

        public string Decompress(string dataToDecompress)
        {
            var decompressed = Decompress(Encoding.Default.GetBytes(dataToDecompress));

            return Encoding.Default.GetString(decompressed);
        }

        private static byte[] Compress(byte[] data, CompressionLevel level = CompressionLevel.Optimal)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var zipStream = new DeflateStream(memoryStream, level, true))
                    zipStream.Write(data, 0, data.Length);
                return memoryStream.ToArray();
            }
        }

        private static byte[] Decompress(byte[] byteData)
        {
            if (byteData == null)
                throw new ArgumentNullException("byteData");

            using (var memoryStream1 = new MemoryStream(byteData))
            {
                using (var memoryStream2 = new MemoryStream())
                {
                    using (var bufferedStream = new BufferedStream(new DeflateStream(memoryStream1, CompressionMode.Decompress), 4096))
                        bufferedStream.CopyTo(memoryStream2);
                    return memoryStream2.ToArray();
                }
            }
        }
    }
}
