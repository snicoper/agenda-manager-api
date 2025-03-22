using AgendaManager.Domain.Common.Messaging.Enums;
using AgendaManager.Domain.Common.Messaging.Interfaces;
using AgendaManager.Infrastructure.Common.Messaging.HostedServices;
using AgendaManager.Infrastructure.Common.Messaging.Interfaces;
using AgendaManager.Infrastructure.Common.Persistence;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace AgendaManager.Infrastructure.UnitTests.Common.Messaging.HostedServices;

public class OutboxMessageProcessorTests
{
    private readonly IAppDbContext _dbContext;
    private readonly IRabbitMqClient _rabbitMqClient;
    private readonly IOutboxMessageRepository _outboxRepository;
    private readonly OutboxMessageProcessor _sut;

    public OutboxMessageProcessorTests()
    {
        var serviceScopeFactory = Substitute.For<IServiceScopeFactory>();
        var serviceScope = Substitute.For<IServiceScope>();
        var serviceProvider = Substitute.For<IServiceProvider>();
        var logger = Substitute.For<ILogger<OutboxMessageProcessor>>();

        _dbContext = Substitute.For<IAppDbContext>();
        _rabbitMqClient = Substitute.For<IRabbitMqClient>();
        _outboxRepository = Substitute.For<IOutboxMessageRepository>();

        // Montamos el scope y el proveedor de servicios.
        serviceScopeFactory.CreateScope().Returns(serviceScope);
        serviceScope.ServiceProvider.Returns(serviceProvider);

        // Montamos los servicios que se resuelven dentro del scope.
        serviceProvider.GetService(typeof(IAppDbContext)).Returns(_dbContext);
        serviceProvider.GetService(typeof(IOutboxMessageRepository)).Returns(_outboxRepository);

        _sut = new OutboxMessageProcessor(_rabbitMqClient, logger);
    }

    [Fact]
    public async Task Should_PublishMessages_When_MessagesAreAvailable()
    {
        // Arrange
        var message = OutboxMessageFactory.CreateOutboxMessage();

        _outboxRepository.GetMessagesForPublishAsync(Arg.Any<CancellationToken>())
            .Returns([message]);

        // Act
        await _sut.ProcessMessagesAsync(_dbContext, _outboxRepository, CancellationToken.None);

        // Assert
        await _rabbitMqClient.Received(1).PublishAsync(
            "test.routing.key",
            "{ \"test\": true }",
            Arg.Any<CancellationToken>());
        await _dbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        message.MessageStatus.Should().Be(OutboxMessageStatus.Published);
    }
}
