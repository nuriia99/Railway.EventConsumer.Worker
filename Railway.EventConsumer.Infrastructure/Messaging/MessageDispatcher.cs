using Railway.EventConsumer.Application.Handlers;
using System.Text.Json;

namespace Railway.EventConsumer.Infrastructure.Messaging
{
    public class MessageDispatcher : IMessageDispatcher
    {
        private readonly Dictionary<string, IMessageHandler> _handlers;

        public MessageDispatcher(IEnumerable<IMessageHandler> handlers)
        {
            _handlers = handlers.ToDictionary(h => h.MessageType);
        }

        public async Task DispatchAsync(string json)
        {
            var doc = JsonDocument.Parse(json);
            var type = doc.RootElement.GetProperty("type").GetString();
            var data = doc.RootElement.GetProperty("data");

            if (!_handlers.TryGetValue(type!, out var handler))
                throw new Exception($"No handler found for message type {type}");

            await handler.HandleAsync(data);
        }
    }

}
