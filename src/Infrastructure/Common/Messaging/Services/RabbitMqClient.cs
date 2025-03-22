using System.Text;
using AgendaManager.Infrastructure.Common.Messaging.Interfaces;
using AgendaManager.Infrastructure.Common.Messaging.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace AgendaManager.Infrastructure.Common.Messaging.Services;

public class RabbitMqClient : IRabbitMqClient, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly RabbitMqSettings _rabbitMqSettings;

    public RabbitMqClient(IOptions<RabbitMqSettings> rabbitMqSettings)
    {
        _rabbitMqSettings = rabbitMqSettings.Value;

        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSettings.Host,
            Port = _rabbitMqSettings.Port,
            UserName = _rabbitMqSettings.User,
            Password = _rabbitMqSettings.Password
        };

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

        _channel.ExchangeDeclareAsync(
            exchange: _rabbitMqSettings.Exchange,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false).GetAwaiter().GetResult();
    }

    public async Task PublishAsync(
        string routingKey,
        string message,
        CancellationToken cancellationToken = default)
    {
        var body = Encoding.UTF8.GetBytes(message);

        await _channel.BasicPublishAsync(
            exchange: _rabbitMqSettings.Exchange,
            routingKey: routingKey,
            body: body,
            cancellationToken: cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
        await _connection.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
