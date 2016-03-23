namespace Tavisca.Frameworks.Logging.Extensions.Infrastructure
{
    /// <summary>
    /// An interface which translates <see cref="IEventEntry"/> and <see cref="IExceptionEntry"/> objects
    /// into string representations.
    /// </summary>
    public interface IEntryStringTranslator
    {
        string TranslateEvent(IEventEntry entry);
        string TranslateException(IExceptionEntry entry);
    }
}