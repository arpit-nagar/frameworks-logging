using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tavisca.Frameworks.Logging.Configuration;
using Tavisca.Frameworks.Logging.TaskScheduling;

namespace Tavisca.Frameworks.Logging.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            //For sample code, refer to the "DoWork" method only

            var factory = TaskScheduling.Utility.GetTaskFactory((IApplicationLogSettings)ConfigurationManager.GetSection("ApplicationLog"),
                false, false);

            var scheduler = factory.Scheduler;

            var propInfo = scheduler.GetType().GetProperty("Activity", BindingFlags.Public | BindingFlags.Instance);

            var activity = (ObservableCollection<string>)propInfo.GetGetMethod().Invoke(scheduler, null);

            activity.CollectionChanged += Activity_CollectionChanged;

            for (int i = 0; i < 50; i++)
            {
                DoWork();
            }

            Console.ReadLine();
        }

        static void Activity_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("Operation: " + e.Action.ToString() + ", Item: " + string.Join(",", e.NewItems.Cast<string>()));
        }

        public static void DoWork()
        {
            IEventEntry entry = null;
            try
            {
                entry = Utility.GetLogEntry();

                entry.Title = "Making a sample of the logging framework.";

                entry.CallType = "Entering some call type.";

                entry.ProviderId = 10; //entered some provider Id.


                //do stuff

                entry.TimeTaken = 10; //entered a time taken
            }
            catch (Exception ex)
            {
                Utility.GetLogger().Write(ex.ToContextualEntry(), null); //null category causes the framework to pick up the default logger.
            }
            finally
            {
                Utility.GetLogger().WriteAsync(entry, KeyStore.Categories.ServiceLevel); //specify a category
                //var stop = false;
                //Utility.GetLogger().WriteAsync(entry, KeyStore.Categories.ServiceLevel, task => stop = true); //specify a category

                //while (!stop)
                //{
                //    System.Threading.Thread.Sleep(1);
                //}
            }
        }
    }
}
