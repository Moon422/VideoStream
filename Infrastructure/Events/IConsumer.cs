using System.Threading.Tasks;

namespace VideoStream.Infrastructure.Events;

public partial interface IConsumer<T>
{
    Task HandleEventAsync(T eventMessage);
}
