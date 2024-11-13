using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using FluentAssertions;
using MediatR;

namespace AgendaManager.Application.UnitTests.Common.Interfaces.Messaging;

public class ICommandTests
{
    [Fact]
    public void Command_ShouldImplementInterface_WhenCheckingIRequestImplementation()
    {
        // Assert
        typeof(ICommand).Should().Implement<IRequest<Result>>();
    }

    [Fact]
    public void Command_ShouldImplementInterface_WhenCheckingIBaseCommandImplementation()
    {
        // Assert
        typeof(ICommand).Should().Implement<IBaseCommand>();
    }

    [Fact]
    public void Command_ShouldImplementInterface_WhenCheckingIAppBaseRequestImplementation()
    {
        // Assert
        typeof(ICommand).Should().Implement<IAppBaseRequest>();
    }

    [Fact]
    public void GenericCommand_ShouldImplementInterface_WhenCheckingIRequestImplementation()
    {
        // Assert
        typeof(ICommand<object>).Should().Implement<IRequest<Result<object>>>();
    }

    [Fact]
    public void GenericCommand_ShouldImplementInterface_WhenCheckingIBaseCommandImplementation()
    {
        // Assert
        typeof(ICommand<object>).Should().Implement<IBaseCommand>();
    }

    [Fact]
    public void GenericCommand_ShouldImplementInterface_WhenCheckingIAppBaseRequestImplementation()
    {
        // Assert
        typeof(ICommand<object>).Should().Implement<IAppBaseRequest>();
    }
}
