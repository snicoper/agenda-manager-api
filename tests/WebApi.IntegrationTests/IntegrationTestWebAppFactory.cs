using AgendaManager.Application.Common.Interfaces.Clock;
using AgendaManager.Infrastructure.Common.Messaging.Options;
using AgendaManager.Infrastructure.Common.Persistence;
using AgendaManager.TestCommon.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace AgendaManager.WebApi.UnitTests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .Build();

    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:3-management-alpine")
        .WithEnvironment("RABBITMQ_DEFAULT_USER", "guest")
        .WithEnvironment("RABBITMQ_DEFAULT_PASS", "guest")
        .WithPortBinding(0, 5672)
        .WithCleanUp(true)
        .Build();

    public async Task InitializeAsync()
    {
        await Task.WhenAll(
            _dbContainer.StartAsync(),
            _rabbitMqContainer.StartAsync());

        await WaitForRabbitMqAsync();
    }

    public new async Task DisposeAsync()
    {
        await Task.WhenAll(
            _dbContainer.DisposeAsync().AsTask(),
            _rabbitMqContainer.DisposeAsync().AsTask());
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureAppConfiguration(
            (_, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:DefaultConnection"] = _dbContainer.GetConnectionString(),
                        ["RabbitMq:Host"] = _rabbitMqContainer.Hostname,
                        ["RabbitMq:Port"] = _rabbitMqContainer.GetMappedPublicPort(5672).ToString(),
                        ["RabbitMq:User"] = "guest",
                        ["RabbitMq:Password"] = "guest",
                        ["RabbitMq:Exchange"] = "agenda.exchange",
                        ["RabbitMq:QueueName"] = "agenda.event.queue"
                    });
            });

        builder.ConfigureTestServices(
            services =>
            {
                var dbContextDescriptor = services
                    .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (dbContextDescriptor is not null)
                {
                    services.Remove(dbContextDescriptor);
                }

                services.AddSingleton<IDateTimeProvider, TestDateTimeProvider>();

                services.AddDbContext<AppDbContext>(
                    (_, options) =>
                    {
                        options.UseNpgsql(_dbContainer.GetConnectionString());
                        options.EnableSensitiveDataLogging();
                    });

                var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(
                        new Dictionary<string, string?>
                        {
                            ["RabbitMq:Host"] = _rabbitMqContainer.Hostname,
                            ["RabbitMq:Port"] = _rabbitMqContainer.GetMappedPublicPort(5672).ToString(),
                            ["RabbitMq:User"] = "guest",
                            ["RabbitMq:Password"] = "guest",
                            ["RabbitMq:Exchange"] = "agenda.exchange",
                            ["RabbitMq:QueueName"] = "agenda.event.queue"
                        })
                    .Build();

                services.Configure<RabbitMqSettings>(config.GetSection("RabbitMq"));
            });
    }

    private async Task WaitForRabbitMqAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqContainer.Hostname,
            Port = _rabbitMqContainer.GetMappedPublicPort(5672),
            UserName = "guest",
            Password = "guest"
        };

        var tries = 20;
        while (tries-- > 0)
        {
            try
            {
                await using var connection = await factory.CreateConnectionAsync();

                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await Task.Delay(1000);
            }
        }

        throw new Exception("RabbitMQ container did not become ready in time.");
    }
}
