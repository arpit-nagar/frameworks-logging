using System.Configuration;

namespace Tavisca.Frameworks.Logging.Configuration
{
    /// <summary>
    /// Collection of <see cref="CategoryElement"/>.
    /// </summary>
    [ConfigurationCollection(typeof(CategoryElement))]
    public sealed class CategoryElementCollection : LogConfigurationElementCollectionBase<CategoryElement>
    {
        #region ConfigurationElementCollection Members

        protected override ConfigurationElement CreateNewElement()
        {
            return new CategoryElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CategoryElement)element).Name;
        }

        #endregion
    }
}
