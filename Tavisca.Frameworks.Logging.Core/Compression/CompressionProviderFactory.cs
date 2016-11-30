using System;

namespace Tavisca.Frameworks.Logging.Compression
{
    /// <summary>
    /// The factory responsible for maintaining the compression provider instance and selection.
    /// </summary>
    public class CompressionProviderFactory
    {
        private static ICompressionProvider _currentProvider;

        /// <summary>
        /// The get or set current compression provider set in the system. If nothing is set, this returns the 
        /// <see cref="DeflateCompressionProvider"/> class instance.
        /// </summary>
        public static ICompressionProvider CurrentProvider
        {
            get
            {
                return _currentProvider ?? (_currentProvider = new DeflateCompressionProvider());
            }
            internal set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _currentProvider = value;
            }
        }
    }
}