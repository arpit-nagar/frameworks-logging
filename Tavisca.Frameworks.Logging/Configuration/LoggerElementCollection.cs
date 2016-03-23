using System.Configuration;

namespace Tavisca.Frameworks.Logging.Configuration
{
    /// <summary>
    /// Collection of <see cref="LoggerElement"/>
    /// </summary>
    [ConfigurationCollection(typeof(LoggerElement))]
    public sealed class LoggerElementCollection : LogConfigurationElementCollectionBase<LoggerElement>
    {
        #region ConfigurationElementCollection Members

        protected override ConfigurationElement CreateNewElement()
        {
            return new LoggerElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LoggerElement)element).Name;
        }

        #endregion
    }
}
