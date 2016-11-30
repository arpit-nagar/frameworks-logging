using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Logging.TaskScheduling
{
    /// <summary>
    /// Contains general utility functions.
    /// </summary>
    public static class Utility
    {
        private static TaskFactory _globalTaskFactory;

        [ThreadStatic]
        private static TaskFactory _localTaskFactory;

        public static TaskFactory GetTaskFactory(Configuration.IApplicationLogSettings settings, bool frameworkDefault, bool localScheduling)
        {
            if (frameworkDefault)
                return Task.Factory;

            if (localScheduling)
            {
                return _localTaskFactory ??
                       (_localTaskFactory = new TaskFactory(TaskSchedulerFactory.GetThreadLocalScheduler(settings)));
            }

            return _globalTaskFactory ??
                (_globalTaskFactory = new TaskFactory(TaskSchedulerFactory.GetGlobalScheduler(settings)));
        }

        public static TaskFactory GetTaskFactory(Configuration.IApplicationLogSettings settings)
        {
            if (settings.MaxThreads < 1)
                return GetTaskFactory(settings, true, false);

            return GetTaskFactory(settings, false, false);
        }

    }
}
