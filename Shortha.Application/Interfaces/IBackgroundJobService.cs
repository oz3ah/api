using System.Linq.Expressions;

namespace Shortha.Application.Interfaces
{
    public interface IBackgroundJobService
    {
        string Enqueue<T>(Expression<Func<T, Task>> methodCall);
        void Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay);

    }
}
