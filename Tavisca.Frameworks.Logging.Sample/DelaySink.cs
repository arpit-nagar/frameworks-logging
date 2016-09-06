using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Logging.Sample
{
    public class DelaySink: SinkBase
    {
        protected override void WriteEvent(ITransactionEntry transactionEntry)
        {
            System.Threading.Thread.Sleep(500);
        }

        protected override void WriteException(IExceptionEntry eventEntry)
        {
            System.Threading.Thread.Sleep(500);
        }
    }
}
