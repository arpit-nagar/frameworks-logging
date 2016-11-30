namespace Tavisca.Frameworks.Logging.Tests.Mock
{
    public class DummyLogger: Logger
    {
        protected override ISink GetLoggerByName(string name)
        {
            return new ExceptionThrowingSink();
        }
    }
}
