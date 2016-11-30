using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Tavisca.Frameworks.Logging.Infrastructure
{
    /// <summary>
    /// Contains extension utility methods for the logging framework, this can be used outside of the framework as well.
    /// </summary>
    public static class Helpers
    {
        //static Helpers()
        //{
        //    Logger.EnsureConfigurationLoad();
        //}

        /// <summary>
        /// Converts the <see cref="Exception"/> into <see cref="IExceptionEntry"/>.
        /// </summary>
        /// <param name="exception">The exception to be converted.</param>
        /// <returns>Returns the exception entry filled with the values of <param name="exception"></param></returns>
        public static IExceptionEntry ToEntry(this Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            return Formatting.FormattingFactory.CurrentFormatter.FormatException(exception);
        }

        public static string Compress(this string dataToCompress)
        {
            return Compression.CompressionProviderFactory.CurrentProvider.Compress(dataToCompress ?? string.Empty);
        }
        public static string DeCompress(this string dataToDecompress)
        {
            return Compression.CompressionProviderFactory.CurrentProvider.Decompress(dataToDecompress);
        }
    }
}
