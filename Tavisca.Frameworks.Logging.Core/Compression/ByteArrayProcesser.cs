using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;

namespace Tavisca.Frameworks.Logging.Compression
{
    public class ByteArrayProcesser
    {
        private static readonly ConcurrentDictionary<DataCompressionLevel, CompressionLevel> CompressionLevelsMapping;

        static ByteArrayProcesser()
        {
            CompressionLevelsMapping = new ConcurrentDictionary<DataCompressionLevel, CompressionLevel>();

            CompressionLevelsMapping
                .AddOrUpdate(DataCompressionLevel.Optimal, CompressionLevel.Optimal, (level, compressionLevel) => CompressionLevel.Optimal);

            CompressionLevelsMapping
                .AddOrUpdate(DataCompressionLevel.Fastest, CompressionLevel.Fastest,(level, compressionLevel) => CompressionLevel.Fastest);

            CompressionLevelsMapping
                .AddOrUpdate(DataCompressionLevel.NoCompression, CompressionLevel.NoCompression, (level, compressionLevel) => CompressionLevel.NoCompression);
        }

        public static byte[] Compress(byte[] data, DataCompressionLevel level = DataCompressionLevel.Fastest)
        {
            using (var memory = new MemoryStream())
            {
                using (var gzip = new GZipStream(memory, CompressionLevelsMapping[level], true))
                {
                    gzip.Write(data, 0, data.Length);
                }
                return memory.ToArray();
            }
        }

        public static byte[] Decompress(byte[] byteData)
        {
            if (byteData == null)
                throw new ArgumentNullException("byteData", @"inputData must be non-null");

            using (var compressedMs = new MemoryStream(byteData))
            {
                using (var decompressedMs = new MemoryStream())
                {
                    using (var gzs = new BufferedStream(new GZipStream(compressedMs, CompressionMode.Decompress), 4096))
                    {
                        gzs.CopyTo(decompressedMs);
                    }
                    return decompressedMs.ToArray();
                }
            }
        }
    }

    public enum DataCompressionLevel
    {
        Optimal = 0,
        Fastest = 1,
        NoCompression = 2
    }
}
