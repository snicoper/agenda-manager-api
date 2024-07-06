using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using FluentAssertions;
using MediatR;

namespace AgendaManager.Application.UnitTests.Common.Interfaces.Messaging;

public class IQueryHandlerTests
{
    [Fact]
    public void IQueryHandler_Should_Be_Implement_Of_IRequestHandler()
    {
        // Assert
        typeof(IQueryHandler<IQuery<object>, object>)
            .Should().Implement<IRequestHandler<IQuery<object>, Result<object>>>();
    }
}
