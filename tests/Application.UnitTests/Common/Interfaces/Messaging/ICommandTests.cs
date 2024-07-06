using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using FluentAssertions;
using MediatR;

namespace AgendaManager.Application.UnitTests.Common.Interfaces.Messaging;

public class ICommandTests
{
    [Fact]
    public void ICommand_Should_Be_Implement_Of_IRequest()
    {
        // Assert
        typeof(ICommand).Should().Implement<IRequest<Result>>();
    }

    [Fact]
    public void ICommand_Should_Be_Implement_Of_IBaseCommand()
    {
        // Assert
        typeof(ICommand).Should().Implement<IBaseCommand>();
    }

    [Fact]
    public void ICommand_Should_Be_Implement_Of_IAppBaseRequest()
    {
        // Assert
        typeof(ICommand).Should().Implement<IAppBaseRequest>();
    }

    [Fact]
    public void ICommand_Generic_Should_Be_Implement_Of_IRequest()
    {
        // Assert
        typeof(ICommand<object>).Should().Implement<IRequest<Result<object>>>();
    }

    [Fact]
    public void ICommand_Generic_Should_Be_Implement_Of_IBaseCommand()
    {
        // Assert
        typeof(ICommand<object>).Should().Implement<IBaseCommand>();
    }

    [Fact]
    public void ICommand_Generic_Should_Be_Implement_Of_IAppBaseRequest()
    {
        // Assert
        typeof(ICommand<object>).Should().Implement<IAppBaseRequest>();
    }
}
