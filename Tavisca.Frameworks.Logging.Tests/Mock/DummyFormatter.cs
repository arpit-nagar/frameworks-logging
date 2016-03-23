using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tavisca.Frameworks.Logging.Formatting;

namespace Tavisca.Frameworks.Logging.Tests.Mock
{
    public class DummyFormatter: DefaultFormatter
    {
        public override IEventEntry FormatEvent(IEventEntry eventEntry)
        {
            eventEntry.AddMessage("This extensibility point is really useful!");

            return base.FormatEvent(eventEntry);
        }

        public override IExceptionEntry FormatException(Exception exception)
        {
            exception.Data.Add("Dummy data key", "This extensibility point is really useful too");

            return base.FormatException(exception);
        }

        public override IExceptionEntry FormatException(IExceptionEntry exceptionEntry)
        {
            exceptionEntry.AddMessage("This extensibility point is really useful!");

            return base.FormatException(exceptionEntry);
        }

        protected override IExceptionEntry FormatException(Exception exception, IExceptionEntry entry)
        {
            return base.FormatException(exception, entry);
        }
    }
}
