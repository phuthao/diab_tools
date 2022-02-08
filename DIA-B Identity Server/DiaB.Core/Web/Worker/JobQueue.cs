using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace DiaB.Core.Web.Worker
{
    public class JobQueue : IJobQueue
    {
        private readonly ConcurrentQueue<Func<IServiceProvider, Task>> _queue;

        private readonly SemaphoreSlim _signal;

        public JobQueue()
        {
            _queue = new ConcurrentQueue<Func<IServiceProvider, Task>>();
            _signal = new SemaphoreSlim(0);
        }

        public async Task<Func<IServiceProvider, Task>> DequeueJobAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);

            _queue.TryDequeue(out var job);

            return job;
        }

        public void EnqueueJob(Func<IServiceProvider, Task> job)
        {
            if (_queue == null)
            {
                throw new ArgumentNullException(nameof(_queue));
            }

            _queue.Enqueue(job);

            _signal.Release();
        }

        public bool IsEmpty()
        {
            return _queue.IsEmpty;
        }
    }
}
