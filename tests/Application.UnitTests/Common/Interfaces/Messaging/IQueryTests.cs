using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using FluentAssertions;
using MediatR;

namespace AgendaManager.Application.UnitTests.Common.Interfaces.Messaging;

public class IQueryTests
{
    [Fact]
    public void IQeury_Should_Be_Implement_Of_IRequest()
    {
        // Assert
        typeof(IQuery<object>).Should().Implement<IRequest<Result<object>>>();
    }

    [Fact]
    public void ICommand_Should_Be_Implement_Of_IQueryBase()
    {
        // Assert
        typeof(IQuery<object>).Should().Implement<IBaseQuery>();
    }

    [Fact]
    public void ICommand_Should_Be_Implement_Of_IAppBaseRequest()
    {
        // Assert
        typeof(IQuery<object>).Should().Implement<IAppBaseRequest>();
    }
}
