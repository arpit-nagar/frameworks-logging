namespace Tavisca.Frameworks.Logging.Extensions.Infrastructure
{
    /// <summary>
    /// An interface which translates <see cref="ITransactionEntry"/> and <see cref="IExceptionEntry"/> objects
    /// into string representations.
    /// </summary>
    public interface IEntryStringTranslator
    {
        string TranslateEvent(ITransactionEntry entry);
        string TranslateException(IExceptionEntry entry);
    }
}