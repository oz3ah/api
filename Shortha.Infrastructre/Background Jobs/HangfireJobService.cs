using Hangfire;
using Shortha.Application.Interfaces;
using System.Linq.Expressions;

namespace Shortha.Infrastructre.Background_Jobs
{

    public class HangfireJobService : IBackgroundJobService
    {
        public string Enqueue<T>(Expression<Func<T, Task>> methodCall)
        {
            return BackgroundJob.Enqueue(methodCall);
        }

        public void Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay)
        {
            BackgroundJob.Schedule(methodCall, delay);
        }
    }
}
