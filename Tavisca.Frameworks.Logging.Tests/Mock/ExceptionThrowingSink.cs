using System;

namespace Tavisca.Frameworks.Logging.Tests.Mock
{
    public class ExceptionThrowingSink: SinkBase
    {
        protected override void WriteTransaction(ITransactionEntry transactionEntry)
        {
            throw new Exception("I have failed event log.", new Exception("This is a test.", new Exception("inner most exception message")));
        }

        protected override void WriteException(IExceptionEntry eventEntry)
        {
            throw new Exception("I have failed event log.", new Exception("This is a test.", new Exception("inner most exception message")));
        }

        protected override IExceptionEntry GetEmptyExceptionEntry()
        {
            return new ExceptionEntry();
        }
    }
}
