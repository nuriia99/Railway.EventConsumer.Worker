using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Railway.EventConsumer.Application.Handlers;
using Railway.EventConsumer.Domain.Events;
using System.Text;
using System.Text.Json;

namespace Railway.EventConsumer.Infrastructure.Messaging
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<RabbitMqConsumer> _logger;
        private readonly IMessageDispatcher _messageDispatcher;

        private IConnection? _connection;
        private IModel? _channel;

        public RabbitMqConsumer(IConfiguration config, 
            IMessageDispatcher messageDispatcher,
            ILogger<RabbitMqConsumer> logger)
        {
            _config = config;
            _logger = logger;
            _messageDispatcher = messageDispatcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connectionString = _config["RabbitMQ:ConnectionString"];
            var queueName = _config["RabbitMQ:QueueName"];

            var factory = new ConnectionFactory { Uri = new Uri(connectionString!) };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, args) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(args.Body.ToArray());

                    await _messageDispatcher.DispatchAsync(json);

                    _channel.BasicAck(args.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                    HandleRetries(args, _channel);
                }
            };

            _channel.BasicConsume(queueName, false, consumer);
            _logger.LogInformation($"Consumer is listening...");

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private void HandleRetries(BasicDeliverEventArgs args, IModel channel)
        {
            var headers = args.BasicProperties.Headers != null
                                    ? new Dictionary<string, object>(args.BasicProperties.Headers)
                                    : new Dictionary<string, object>();

            int retryCount = headers.ContainsKey("x-retry-count")
                ? Convert.ToInt32(headers["x-retry-count"])
                : 0;

            if (retryCount < 3)
            {
                var props = channel.CreateBasicProperties();
                props.Headers = headers;
                props.Headers["x-retry-count"] = retryCount + 1;

                channel.BasicPublish(
                    exchange: "retry-exchange",
                    routingKey: "retry",
                    basicProperties: props,
                    body: args.Body);

                channel.BasicAck(args.DeliveryTag, false);
                _logger.LogWarning($"Message send to retry: {retryCount + 1}/3");
            }
            else
            {
                channel.BasicNack(args.DeliveryTag, false, false);
                _logger.LogError("Message discard after 3 retries");
            }
        }
    }
}
