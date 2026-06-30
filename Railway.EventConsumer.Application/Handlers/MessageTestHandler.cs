using Railway.EventConsumer.Domain.Events;
using Railway.EventConsumer.Domain.Exceptions;

namespace Railway.EventConsumer.Application.Handlers
{
    public class MessageTestHandler : IMessageTestHandler
    {
        public Task HandleAsync(MessageTest message, CancellationToken ct)
        {
            Console.WriteLine($"Procesando mensaje {message.MessageId}");

            if (message.Amount < 0)
            {
                throw new TestException("Test retry policy");
            }

            return Task.CompletedTask;
        }
    }
}
