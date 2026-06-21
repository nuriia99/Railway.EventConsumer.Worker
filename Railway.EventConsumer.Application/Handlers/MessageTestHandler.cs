using Railway.EventConsumer.Domain.Events;

namespace Railway.EventConsumer.Application.Handlers
{
    public class MessageTestHandler : IMessageTestHandler
    {
        public Task HandleAsync(MessageTest message, CancellationToken ct)
        {
            Console.WriteLine($"Procesando mensaje {message.MessageId}");
            return Task.CompletedTask;
        }
    }
}
