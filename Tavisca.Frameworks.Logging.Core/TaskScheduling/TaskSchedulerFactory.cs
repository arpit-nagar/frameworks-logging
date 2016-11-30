using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.Frameworks.Logging.Configuration;

namespace Tavisca.Frameworks.Logging.TaskScheduling
{
    /// <summary>
    /// Scheduler factory, responsible for creating and maintaining <see cref="TaskScheduler"/> classes.
    /// </summary>
    internal static class TaskSchedulerFactory
    {
        #region Fields & Properties

        private static bool _isConfigSet;

        private static TaskScheduler _globalScheduler;

        [ThreadStatic]
        private static TaskScheduler _threadStaticScheduler;

        public static int MaxDegreeOfParallelism { get; private set; }

        public static bool UseWorkerProcessThreadPool { get; private set; }

        #endregion

        #region Public Methods

        public static TaskScheduler GetGlobalScheduler(IApplicationLogSettings config)
        {
            SetTaskSchedulerFactory(config);

            return _globalScheduler;
        }

        public static TaskScheduler GetThreadLocalScheduler(IApplicationLogSettings config)
        {
            SetTaskSchedulerFactory(config);

            return _threadStaticScheduler ??
                   (_threadStaticScheduler = CreateScheduler());
        }

        #endregion

        #region Private Methods

        private static void SetTaskSchedulerFactory(IApplicationLogSettings config)
        {
            if (_isConfigSet)
                return;

            _isConfigSet = true;
            try
            {
                MaxDegreeOfParallelism = config.MaxThreads < 1 ? 50 : config.MaxThreads;
            }
            catch
            {
                MaxDegreeOfParallelism = 50;
            }

            UseWorkerProcessThreadPool = config.UseWorkerProcessThreads;

            _globalScheduler = CreateScheduler();
        }

        private static TaskScheduler CreateScheduler()
        {
            return new LimitedConcurrencyLevelTaskScheduler(MaxDegreeOfParallelism, UseWorkerProcessThreadPool);
        }

        #endregion
    }
}
