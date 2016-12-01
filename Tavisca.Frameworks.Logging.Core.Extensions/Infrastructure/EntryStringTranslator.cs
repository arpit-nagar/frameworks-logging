using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tavisca.Frameworks.Logging.Extensions.Infrastructure
{
    /// <summary>
    /// Default implementation of <see cref="IEntryStringTranslator"/>, uses reflection
    /// to dump the object into a specific format.
    /// </summary>
    public class EntryStringTranslator : IEntryStringTranslator
    {
        #region Constants

        protected const string LineDelimiter = "";
        protected const string EntryDelimiter = "**************************************************";
        protected const string KeyDelimiter = " -> ";

        #endregion

        #region IEntryStringTranslator Members

        public string TranslateTransaction(ITransactionEntry entry)
        {
            return DumpObject(entry);
        }

        public string TranslateException(IExceptionEntry entry)
        {
            return DumpObject(entry);
        }

        public string TranslateEvent(IEventEntry entry)
        {
            return DumpObject(entry);
        }

        #endregion

        #region Protected Methods

        protected virtual string DumpObject(object obj)
        {
            var builder = new StringBuilder();

            var type = obj.GetType();

            var properties = type.GetTypeInfo().GetProperties().Select(x => new
            {
                x.Name,
                Method = x.GetGetMethod()
            })
                .Where(x => x.Method != null);

            builder.Append("Object Type: ").AppendLine(type.Name);

            foreach (var propInfo in properties)
            {
                builder
                    .AppendLine(LineDelimiter)
                    .Append(propInfo.Name)
                    .Append(KeyDelimiter);
                try
                {
                    builder
                        .AppendLine(GetString(propInfo.Method.Invoke(obj, null)));
                }
                catch { }

            }

            builder.AppendLine(EntryDelimiter);

            return builder.ToString();
        }

        #endregion

        #region Private Methods

        private static string GetString(object obj)
        {
            if (obj == null)
                return string.Empty;

            var dictionary = obj as IDictionary<string, string>;

            if (dictionary != null)
            {
                if (dictionary.Count == 0)
                    return string.Empty;

                var builder = new StringBuilder();

                builder.AppendLine(string.Empty);

                foreach (var pair in dictionary)
                {
                    builder.Append(pair.Key).Append(":").AppendLine(pair.Value);
                }
                builder.AppendLine("-----------------------------------------------");

                return builder.ToString();
            }

            return obj.ToString();
        }

        #endregion
    }
}
