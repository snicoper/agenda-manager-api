using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using FluentAssertions;
using MediatR;

namespace AgendaManager.Application.UnitTests.Common.Interfaces.Messaging;

public class IQueryTests
{
    [Fact]
    public void Query_ShouldImplementInterface_WhenCheckingIRequestImplementation()
    {
        // Assert
        typeof(IQuery<object>).Should().Implement<IRequest<Result<object>>>();
    }

    [Fact]
    public void Query_ShouldImplementInterface_WhenCheckingIQueryBaseImplementation()
    {
        // Assert
        typeof(IQuery<object>).Should().Implement<IBaseQuery>();
    }

    [Fact]
    public void Query_ShouldImplementInterface_WhenCheckingIAppBaseRequestImplementation()
    {
        // Assert
        typeof(IQuery<object>).Should().Implement<IAppBaseRequest>();
    }
}
