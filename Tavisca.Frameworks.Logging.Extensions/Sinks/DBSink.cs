using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using Tavisca.Frameworks.Logging.Extensions.Data;
using Tavisca.Frameworks.Logging.Extensions.Data.Translation;

namespace Tavisca.Frameworks.Logging.Extensions.Sinks
{
    /// <summary>
    /// A database sink implementation, pushes the <see cref="ILogEntry"/> objects into the database 
    /// in a specific schema. The class can be inherited, override the <seealso cref="GetLoggingContext"/>
    /// method to specify a different connection string. Default connection string name: "log".
    /// </summary>
    public class DBSink : SinkBase
    {
        private readonly EntryToModelTranslator _translator;

        #region Constructors

        public DBSink()
        {
            _translator = new EntryToModelTranslator();
        }

        #endregion

        #region SinkBase Members

        protected override void WriteTransaction(ITransactionEntry transactionEntry)
        {
            var model = _translator.ToModel(transactionEntry);

            try
            {
                using (var context = GetLoggingContext())
                {
                    context.Logs.Add(model);

                    context.SaveChanges();
                }
            }
            catch(DbEntityValidationException ex)
            {
                AddErrorInfoToException(ex);
                throw;
            }
        }

        protected override void WriteException(IExceptionEntry eventEntry)
        {
            var model = _translator.ToModel(eventEntry);

            try
            {
                using (var context = GetLoggingContext())
                {
                    context.ExceptionLogs.Add(model);

                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                AddErrorInfoToException(ex);
                throw;
            }
        }

        #endregion

        #region Protected Methods

        protected virtual LoggingContext GetLoggingContext()
        {
            return new LoggingContext();
        }

        #endregion

        #region Private Methods

        private void AddErrorInfoToException(DbEntityValidationException dbEntityValidationException)
        {
            foreach (var error in dbEntityValidationException.EntityValidationErrors.SelectMany(x => x.ValidationErrors))
            {
                dbEntityValidationException.Data.Add(error.PropertyName, error.ErrorMessage);
            }
        }

        #endregion
    }
}
