using DiaB.Middle.Services.Interfaces;
using Quartz;
using System.Threading.Tasks;

namespace DiaB.WebApi.Jobs
{
    public class NotificationJob : IJob
    {

        public NotificationJob()
        {

        }

        public async Task Execute(IJobExecutionContext context)
        {
            
        }
    }
}
