using System.Configuration;

namespace Tavisca.Frameworks.Logging.Configuration
{
    /// <summary>
    /// Defines a named category, contains collection of <see cref="LoggerElement"/> which actually do the 
    /// logging for this section.
    /// </summary>
    public sealed class CategoryElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the name of the category which will be passed to the <see cref="ILogger"/>
        /// while logging.
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets a collection of <see cref="LoggerElement"/> , this defines the loggers which will 
        /// log in this category.
        /// </summary>
        [ConfigurationProperty("loggers", IsDefaultCollection = true)]
        public LoggerElementCollection Loggers
        {
            get { return (LoggerElementCollection)this["loggers"]; }
            set { this["loggers"] = value; }
        }
    }
}
