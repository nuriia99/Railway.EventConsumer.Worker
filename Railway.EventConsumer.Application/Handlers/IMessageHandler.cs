using System.Text.Json;

namespace Railway.EventConsumer.Application.Handlers
{
    public interface IMessageHandler
    {
        string MessageType { get; }
        Task HandleAsync(JsonElement data);
    }
}
