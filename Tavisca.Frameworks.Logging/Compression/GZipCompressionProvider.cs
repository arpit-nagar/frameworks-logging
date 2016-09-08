using System;
using System.IO.Compression;
using System.Text;
using Tavisca.Frameworks.Helper;

namespace Tavisca.Frameworks.Logging.Compression
{
    public class GZipCompressionProvider : ICompressionProvider
    {
        public string Compress(string dataToCompress)
        {
            var compressed = ByteArrayProcesser.Compress(Encoding.UTF8.GetBytes(dataToCompress), DataCompressionLevel.Optimal);

            return Convert.ToBase64String(compressed);
        }

        public string Decompress(string dataToDecompress)
        {
            var decompressed = ByteArrayProcesser.Decompress(Convert.FromBase64String(dataToDecompress));

            return Encoding.UTF8.GetString(decompressed);
        }
    }
}
