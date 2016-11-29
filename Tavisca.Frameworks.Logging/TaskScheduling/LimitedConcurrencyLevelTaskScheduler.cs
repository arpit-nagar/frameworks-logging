using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Logging.TaskScheduling
{
    /// <summary>
    /// A task scheduler which limits the concurrency of threads produced to a certain specified limit.
    /// </summary>
    internal sealed class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
    {
        /// <summary>Whether the current thread is processing work items.</summary>
        [ThreadStatic]
        private static bool _currentThreadIsProcessingItems;
        /// <summary>The list of tasks to be executed.</summary> 
        private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks) 
        /// <summary>The maximum concurrency level allowed by this scheduler.</summary> 
        private readonly int _maxDegreeOfParallelism;
        /// <summary>Whether the scheduler is currently processing work items.</summary> 
        private int _delegatesQueuedOrRunning; // protected by lock(_tasks) 

        /// <summary>Whether activity log is enabled, this is for monitoring purposes.</summary>
        private readonly bool _logEnabled;
        /// <summary>How many threads are currently in queued state or in execution state.</summary>
        public int RunningThreads { get { return _delegatesQueuedOrRunning; } }
        /// <summary>How many tasks are currently in queued state.</summary>
        public int PendingTasks { get { return _tasks.Count; } }

        public bool UseWorkerProcessThreadPool { get; set; }

        public ObservableCollection<string> Activity { get; set; }
        /// <summary> 
        /// Initializes an instance of the LimitedConcurrencyLevelTaskScheduler class with the 
        /// specified degree of parallelism. 
        /// </summary> 
        /// <param name="maxDegreeOfParallelism">The maximum degree of parallelism provided by this scheduler.</param>
        /// <param name="useWorkerProcessThreadPool">whether the scheduler should use worker process thread pool or create new threads.</param>
        public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism, bool useWorkerProcessThreadPool)
        {
            UseWorkerProcessThreadPool = useWorkerProcessThreadPool;
            if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
            _maxDegreeOfParallelism = maxDegreeOfParallelism;

            //<NOTE: .net core changes > ConfigurationManager is not supported in .net core, code need to be configurable
            //_logEnabled = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["LimitedConcurrencyLog"]);
            _logEnabled = false;
            
            if (_logEnabled)
            {
                Activity = new ObservableCollection<string>();

                NotifyActivity("LimitedConcurrencyLevelTaskScheduler initialized successfully.");
            }
        }

        /// <summary>Queues a task to the scheduler.</summary> 
        /// <param name="task">The task to be queued.</param>
        protected sealed override void QueueTask(Task task)
        {
            // Add the task to the list of tasks to be processed.  If there aren't enough 
            // delegates currently queued or running to process tasks, schedule another. 
            lock (_tasks)
            {
                _tasks.AddLast(task);
                NotifyActivity("A task has been added.");
                if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
                {
                    ++_delegatesQueuedOrRunning;
                    NotifyThreadPoolOfPendingWork();
                    NotifyActivity("A thread was successfully spawned.");
                }
            }
        }

        /// <summary> 
        /// Informs the ThreadPool that there's work to be executed for this scheduler. 
        /// </summary> 
        private void NotifyThreadPoolOfPendingWork()
        {
            Action pendingWork = () =>
            {
                // Note that the current thread is now processing work items. 
                // This is necessary to enable inlining of tasks into this thread.
                _currentThreadIsProcessingItems = true;
                try
                {
                    // Process all available items in the queue. 
                    while (true)
                    {
                        Task item;
                        lock (_tasks)
                        {
                            // When there are no more items to be processed, 
                            // note that we're done processing, and get out. 
                            if (_tasks.Count == 0)
                            {
                                --_delegatesQueuedOrRunning;
                                break;
                            }

                            // Get the next item from the queue
                            item = _tasks.First.Value;
                            _tasks.RemoveFirst();
                        }
                        NotifyActivity("A task is about to be executed.");
                        // Execute the task we pulled out of the queue 
                        base.TryExecuteTask(item);

                        NotifyActivity("A task was executed");
                    }
                }
                // We're done processing items on the current thread 
                finally { _currentThreadIsProcessingItems = false; }
            };

            if (UseWorkerProcessThreadPool)
            {
                //<CRITICAL .net core changes> UnsafeQueueUserWorkItem is not supported in .net core, code need to be revisited
                //ThreadPool.UnsafeQueueUserWorkItem(_ => pendingWork(), null);
                ThreadPool.QueueUserWorkItem(_ => pendingWork(), null);
            }
            else
            {
                var thread = new Thread(() => pendingWork());

                thread.Start();
            }
        }

        /// <summary>Attempts to execute the specified task on the current thread.</summary> 
        /// <param name="task">The task to be executed.</param>
        /// <param name="taskWasPreviouslyQueued"></param>
        /// <returns>Whether the task could be executed on the current thread.</returns> 
        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            // If this thread isn't already processing a task, we don't support inlining 
            if (!_currentThreadIsProcessingItems) return false;

            // If the task was previously queued, remove it from the queue 
            if (taskWasPreviouslyQueued) TryDequeue(task);

            NotifyActivity("A task was executed synchronously as requested by the framework.");
            // Try to run the task. 
            return base.TryExecuteTask(task);
        }

        /// <summary>Attempts to remove a previously scheduled task from the scheduler.</summary> 
        /// <param name="task">The task to be removed.</param>
        /// <returns>Whether the task could be found and removed.</returns> 
        protected sealed override bool TryDequeue(Task task)
        {
            lock (_tasks) return _tasks.Remove(task);
        }

        /// <summary>Gets the maximum concurrency level supported by this scheduler.</summary> 
        public sealed override int MaximumConcurrencyLevel { get { return _maxDegreeOfParallelism; } }

        /// <summary>Gets an enumerable of the tasks currently scheduled on this scheduler.</summary> 
        /// <returns>An enumerable of the tasks currently scheduled.</returns> 
        protected sealed override IEnumerable<Task> GetScheduledTasks()
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(_tasks, ref lockTaken);
                if (lockTaken) return _tasks.ToArray();
                else throw new NotSupportedException();
            }
            finally
            {
                if (lockTaken) Monitor.Exit(_tasks);
            }
        }

        private void NotifyActivity(string details)
        {
            if (!_logEnabled)
                return;

            var curentThread = Thread.CurrentThread;

            //Activity.Add(details + ":: Thread Id: " + curentThread.ManagedThreadId + " Thread pool thread: " + curentThread.IsThreadPoolThread);
            
            //<CRITICAL> IsThreadPoolThread is not supported in .net core, code need to be revisited
            Activity.Add(details + ":: Thread Id: " + curentThread.ManagedThreadId + " Thread Name: " + curentThread.Name);
        }
    }
}
