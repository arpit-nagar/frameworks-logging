using Tavisca.Frameworks.Logging.DependencyInjection;
using Tavisca.Frameworks.Logging.Extensions.Infrastructure;

namespace Tavisca.Frameworks.Logging.Extensions.Sinks
{
    /// <summary>
    /// Acts as a base class for all sinks which logs the objects in a singular string representation of the same.
    /// </summary>
    public abstract class StringWritingSinkBase : SinkBase
    {
        #region Protected Members

        protected virtual IEntryStringTranslator GetTranslator()
        {
            return LocatorProvider.GetContainer().GetInstance<IEntryStringTranslator>();
        }

        #endregion
    }
}
