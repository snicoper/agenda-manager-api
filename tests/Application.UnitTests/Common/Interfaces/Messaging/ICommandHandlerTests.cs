using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace AgendaManager.Application.UnitTests.Common.Interfaces.Messaging;

public class ICommandHandlerTests
{
    [Fact]
    public void ICommandHandler_ShouldImplementIRequestHandler_WithResultType()
    {
        // Arrange
        var commandHandlerSub = Substitute.For<ICommandHandler<ICommand>>();

        // Assert
        commandHandlerSub.Should().BeAssignableTo<IRequestHandler<ICommand, Result>>();
    }

    [Fact]
    public void ICommandHandlerGeneric_ShouldImplementIRequestHandler_WithResultGenericType()
    {
        // Arrange
        var commandHandlerSub = Substitute.For<ICommandHandler<ICommand<string>, string>>();

        // Assert
        commandHandlerSub.Should().BeAssignableTo<IRequestHandler<ICommand<string>, Result<string>>>();
    }
}
