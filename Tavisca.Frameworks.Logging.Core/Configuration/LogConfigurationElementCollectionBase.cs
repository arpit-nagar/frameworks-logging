using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Tavisca.Frameworks.Logging.Configuration
{
    /// <summary>
    /// Serves as a base class for all ConfigurationElementCollection objects which adds helper functions into it.
    /// </summary>
    /// <typeparam name="T">The type of ConfigurationElement that the implementor class is a collection of.</typeparam>
    public abstract class LogConfigurationElementCollectionBase<T> : ConfigurationElementCollection
        where T: ConfigurationElement
    {
        #region Public Members

        /// <summary>
        /// Adds the element into the collection, throws an error if the element already exists (matched by key).
        /// </summary>
        /// <param name="element"></param>
        public void Add(T element)
        {
            this.BaseAdd(element, true);
        }

        /// <summary>
        /// Removes an element from the collection by matching keys.
        /// </summary>
        /// <param name="element">The element to remove.</param>
        public void Remove(T element)
        {
            var key = this.GetElementKey(element);

            this.BaseRemove(element);
        }

        #endregion
    }
}
