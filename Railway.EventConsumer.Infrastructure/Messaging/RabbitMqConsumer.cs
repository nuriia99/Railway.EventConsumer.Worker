using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Railway.EventConsumer.Application.Handlers;
using Railway.EventConsumer.Domain.Events;
using System.Text;
using System.Text.Json;

namespace Railway.EventConsumer.Infrastructure.Messaging
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly IMessageTestHandler _handler;
        private readonly ILogger<RabbitMqConsumer>? _logger;

        private IConnection? _connection;
        private IModel? _channel;

        public RabbitMqConsumer(IConfiguration config, IMessageTestHandler handler, ILogger<RabbitMqConsumer>? logger = null)
        {
            _config = config;
            _handler = handler;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connectionString = _config["RabbitMQ:ConnectionString"];
            var queueName = _config["RabbitMQ:Queue"];

            if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(queueName))
            {
                _logger?.LogError("RabbitMQ configuration missing (RabbitMQ:ConnectionString or RabbitMQ:Queue)");
                return;
            }

            try
            {
                var factory = new RabbitMQ.Client.ConnectionFactory { Uri = new Uri(connectionString) };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(_channel);

                consumer.Received += async (sender, args) =>
                {
                    try
                    {
                        var json = Encoding.UTF8.GetString(args.Body.ToArray());
                        var message = JsonSerializer.Deserialize<MessageTest>(json);

                        if (message != null)
                        {
                            await _handler.HandleAsync(message, stoppingToken).ConfigureAwait(false);
                        }

                        _channel!.BasicAck(args.DeliveryTag, false);
                    }
                    catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                    {
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Error processing RabbitMQ message");
                        try { _channel?.BasicNack(args.DeliveryTag, false, true); } catch { }
                    }
                };

                _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

                try { await Task.Delay(Timeout.Infinite, stoppingToken).ConfigureAwait(false); } catch (TaskCanceledException) { }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Fatal error in RabbitMQ consumer");
                throw;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
