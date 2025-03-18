using System.Text;
using AgendaManager.Infrastructure.Common.Messaging.Interfaces;
using AgendaManager.Infrastructure.Common.Messaging.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AgendaManager.Infrastructure.Common.Messaging.HostedServices;

public class RabbitMqConsumerHostedService : BackgroundService
{
    private readonly ILogger<RabbitMqConsumerHostedService> _logger;
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly IIntegrationEventDispatcher _dispatcher;
    private readonly RabbitMqSettings _settings;

    public RabbitMqConsumerHostedService(
        ILogger<RabbitMqConsumerHostedService> logger,
        IOptions<RabbitMqSettings> settings,
        IIntegrationEventDispatcher dispatcher)
    {
        _logger = logger;
        _dispatcher = dispatcher;
        _settings = settings.Value;

        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            UserName = _settings.User,
            Password = _settings.Password
        };

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public override void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();

        base.Dispose();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RabbitMqConsumerHostedService started");

        await _channel.ExchangeDeclareAsync(
            exchange: _settings.Exchange,
            type: ExchangeType.Direct,
            durable: true,
            cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync(
            queue: _settings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: stoppingToken);

        await _channel.QueueBindAsync(
            queue: _settings.QueueName,
            exchange: _settings.Exchange,
            routingKey: "#",
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var routingKey = ea.RoutingKey;

            _logger.LogInformation("Mensaje recibido: {RoutingKey} - {Message}", routingKey, message);

            try
            {
                await _dispatcher.DispatchAsync(routingKey, message, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al despachar evento {RoutingKey}", routingKey);
            }
        };

        await _channel.BasicConsumeAsync(queue: _settings.QueueName, autoAck: true, consumer: consumer, stoppingToken);
    }
}
