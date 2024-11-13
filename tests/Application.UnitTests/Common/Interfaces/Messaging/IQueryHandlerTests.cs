using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using FluentAssertions;
using MediatR;

namespace AgendaManager.Application.UnitTests.Common.Interfaces.Messaging;

public class IQueryHandlerTests
{
    [Fact]
    public void QueryHandler_ShouldImplementInterface_WhenCheckingIRequestHandler()
    {
        // Assert
        typeof(IQueryHandler<IQuery<object>, object>)
            .Should().Implement<IRequestHandler<IQuery<object>, Result<object>>>();
    }
}
