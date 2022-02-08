using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiaB.Core.Web.Worker
{
    public interface IJobQueue
    {
        bool IsEmpty();

        void EnqueueJob(Func<IServiceProvider, Task> job);

        Task<Func<IServiceProvider, Task>> DequeueJobAsync(CancellationToken cancellationToken);
    }
}
