using System.Threading.Tasks;

namespace VideoStream.Application.Events;

public interface IConsumer<T>
{
    Task HandleEventAsync(T eventMessage);
}