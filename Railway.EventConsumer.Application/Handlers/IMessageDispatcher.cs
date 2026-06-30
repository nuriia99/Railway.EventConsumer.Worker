namespace Railway.EventConsumer.Application.Handlers
{
    public interface IMessageDispatcher
    {
        Task DispatchAsync(string json);
    }
}