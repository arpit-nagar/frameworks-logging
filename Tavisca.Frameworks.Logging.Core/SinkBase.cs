using System;
using System.Threading.Tasks;
using Tavisca.Frameworks.Logging.DependencyInjection;
using Tavisca.Frameworks.Logging.Exceptions;
using Tavisca.Frameworks.Logging.Infrastructure;
using Tavisca.Frameworks.Logging.Resources;

namespace Tavisca.Frameworks.Logging
{
    public abstract class SinkBase : ISink
    {
        #region ISink Methods

        public void WriteAsync(ILogEntry entry)
        {
            try
            {
                if (entry == null)
                    throw new ArgumentNullException("entry");

                var task = new Task(() => this.Write(entry));

                task.Start();
            }
            catch (LogException)
            {
                throw;
            }
            catch (Exception ex)
            {
                FailSafeLog(ex);
            }
        }

        public void WriteAsync(ILogEntry entry, Action<Task> continueWith)
        {
            try
            {
                if (entry == null)
                    throw new ArgumentNullException("entry");

                if (continueWith == null)
                    throw new ArgumentNullException("continueWith");

                var task = new Task(() => this.Write(entry));

                task.ContinueWith(continueWith);

                task.Start();
            }
            catch (LogException)
            {
                throw;
            }
            catch (Exception ex)
            {
                FailSafeLog(ex);
            }
        }

        public void WriteExceptionAsync(Exception exception)
        {
            try
            {
                if (exception == null)
                    throw new ArgumentNullException("exception");

                var task = new Task(() => this.WriteException(exception));

                task.Start();
            }
            catch (LogException)
            {
                throw;
            }
            catch (Exception ex)
            {
                FailSafeLog(ex);
            }
        }

        public void WriteExceptionAsync(Exception exception, Action<Task> continueWith)
        {
            try
            {
                if (exception == null)
                    throw new ArgumentNullException("exception");

                if (continueWith == null)
                    throw new ArgumentNullException("continueWith");

                var task = new Task(() => this.WriteException(exception));

                task.ContinueWith(continueWith);

                task.Start();
            }
            catch (LogException)
            {
                throw;
            }
            catch (Exception ex)
            {
                FailSafeLog(ex);
            }
        }

        public virtual void WriteException(Exception exception)
        {
            try
            {
                var entry = exception.ToEntry();

                this.Write(entry);
            }
            catch (LogException)
            {
                throw;
            }
            catch (Exception ex)
            {
                FailSafeLog(ex);
            }
        }

        public virtual void Write(ILogEntry entry)
        {
            try
            {
                if (entry is ITransactionEntry)
                {
                    WriteTransaction((ITransactionEntry)entry);
                }
                else if (entry is IExceptionEntry)
                {
                    WriteException((IExceptionEntry)entry);
                }
                else if (entry is IEventEntry)
                {
                    WriteEvent((IEventEntry)entry);
                }
                else
                {
                    throw new NotSupportedException(string.Format(LogResources.UnSupportedLogEntryType, entry.GetType().AssemblyQualifiedName));
                }
            }
            catch (LogException)
            {
                throw;
            }
            catch (Exception ex)
            {
                FailSafeLog(ex);
            }
        }

        #endregion

        #region Protected Methods

        protected virtual IExceptionEntry GetEmptyExceptionEntry()
        {
            return LocatorProvider.GetContainer().GetInstance<IExceptionEntry>();
        }

        protected void FailSafeLog(Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException("ex");

            FailSafeLogFactory.Log(ex);
        }

        protected abstract void WriteTransaction(ITransactionEntry transactionEntry);

        protected abstract void WriteException(IExceptionEntry eventEntry);

        protected abstract void WriteEvent(IEventEntry transactionEntry);

        #endregion
    }
}
