using Railway.EventConsumer.Domain.Events;

namespace Railway.EventConsumer.Application.Handlers
{
    public interface IMessageTestHandler
    {
        Task HandleAsync(MessageTest message, CancellationToken ct);
    }
}
