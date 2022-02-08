using Microsoft.Extensions.DependencyInjection;

namespace DiaB.Core.Web.Worker
{
    public static class WorkerExtension
    {
        public static IServiceCollection AddWorker(this IServiceCollection services)
        {
            services.AddSingleton<IJobQueue, JobQueue>();
            services.AddHostedService<Worker>();

            return services;
        }
    }
}
