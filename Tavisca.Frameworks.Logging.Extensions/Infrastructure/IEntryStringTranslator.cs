namespace Tavisca.Frameworks.Logging.Extensions.Infrastructure
{
    /// <summary>
    /// An interface which translates <see cref="ITransactionEntry"/> <see cref="IEventEntry"/> and <see cref="IExceptionEntry"/> objects
    /// into string representations.
    /// </summary>
    public interface IEntryStringTranslator
    {
        string TranslateTransaction(ITransactionEntry entry);
        string TranslateException(IExceptionEntry entry);
        string TranslateEvent(IEventEntry entry);
    }
}