using System.Configuration;

namespace Tavisca.Frameworks.Logging.Configuration
{
    /// <summary>
    /// Defines the <see cref="ISink"/> used for logging.
    /// </summary>
    public sealed class LoggerElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the key of the <see cref="ISink"/> as configured in an IOC container.
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
    }
}
