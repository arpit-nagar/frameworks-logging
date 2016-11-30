namespace Tavisca.Frameworks.Logging.Configuration
{
    /// <summary>
    /// Defines the <see cref="ISink"/> used for logging.
    /// </summary>
    public sealed class LoggerElement
    {
        /// <summary>
        /// Gets or sets the key of the <see cref="ISink"/> as configured in an IOC container.
        /// </summary>
        public string Name { get; set; }
    }
}
