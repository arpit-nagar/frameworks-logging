using System.Collections.Generic;

namespace Tavisca.Frameworks.Logging.Configuration
{
    /// <summary>
    /// Defines a named category, contains collection of <see cref="LoggerElement"/> which actually do the 
    /// logging for this section.
    /// </summary>
    public class CategoryElement
    {
        /// <summary>
        /// Gets or sets the name of the category which will be passed to the <see cref="ILogger"/>
        /// while logging.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a collection of <see cref="LoggerElement"/> , this defines the loggers which will 
        /// log in this category.
        /// </summary>
        public List<LoggerElement> Loggers { get; set; }
    }
}
