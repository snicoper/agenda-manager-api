using System.Net.Http.Json;
using AgendaManager.Domain.Common.Messaging;
using AgendaManager.Domain.Common.Messaging.Enums;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Infrastructure.Common.Persistence;
using AgendaManager.TestCommon.Constants;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaManager.WebApi.UnitTests.DistributedSystem;

public class ForgotPasswordEventFlowTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_StoreEventInOutbox_WithStatePublished()
    {
        // Arrange
        var request = new { Email = UserConstants.UserAlice.Email.Value };

        // Act
        await HttpClient.PostAsJsonAsync(Enpoints.UsersAccounts.RequestPasswordReset, request);

        // Assert
        using var scope = Factory.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var message = await WaitForPublishedOutboxMessage(nameof(UserTokenCreatedDomainEvent), Factory.Services);

        message.Should().NotBeNull("event should have been marked as Published in the Outbox");
        message!.MessageStatus.Should().Be(OutboxMessageStatus.Published);
    }

    private static async Task<OutboxMessage?> WaitForPublishedOutboxMessage(
        string eventType,
        IServiceProvider serviceProvider,
        int maxRetries = 100,
        int delayMs = 1000)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        OutboxMessage? message = null;

        for (var i = 0; i < maxRetries; i++)
        {
            Console.WriteLine(i);
            message = await dbContext.OutboxMessages
                .OrderByDescending(om => om.PublishedOn)
                .FirstOrDefaultAsync(
                    om =>
                        om.Type.Contains(eventType) &&
                        om.MessageStatus == OutboxMessageStatus.Published);

            if (message is not null)
            {
                break;
            }

            await Task.Delay(delayMs);
        }

        return message;
    }
}
