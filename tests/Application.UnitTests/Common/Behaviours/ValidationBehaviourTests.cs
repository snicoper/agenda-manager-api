using AgendaManager.Application.Common.Behaviours;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace AgendaManager.Application.UnitTests.Common.Behaviours;

public class ValidationBehaviourTests
{
    private readonly ILogger<Result> _logger;
    private readonly ValidationBehaviour<TestRequest, Result> _sut;
    private readonly IValidator<TestRequest> _validator;

    public ValidationBehaviourTests()
    {
        _logger = Substitute.For<ILogger<Result>>();
        _validator = Substitute.For<IValidator<TestRequest>>();
        _sut = new ValidationBehaviour<TestRequest, Result>(_logger, _validator);
    }

    [Fact]
    public async Task Handle_ShouldCallNext_WhenValidatorIsNull()
    {
        // Arrange
        var request = new TestRequest();
        var next = Substitute.For<RequestHandlerDelegate<Result>>();
        _validator.ValidateAsync(request, CancellationToken.None).Returns(new ValidationResult());

        // Act
        await _sut.Handle(request, next, CancellationToken.None);

        // Assert
        await next.Received(1).Invoke();
    }

    [Fact]
    public async Task Handle_ShouldReturnResult_WhenValidationSucceeds()
    {
        // Arrange
        var request = new TestRequest();
        _validator.ValidateAsync(request, CancellationToken.None).Returns(new ValidationResult());

        // Act
        var result = await _sut.Handle(request, () => Task.FromResult(Result.Success()), CancellationToken.None);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Error?.HasErrors.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldReturnErrorResult_WhenValidationFails()
    {
        // Arrange
        var request = new TestRequest();
        _validator.ValidateAsync(request, CancellationToken.None)
            .Returns(new ValidationResult([new ValidationFailure("Property", "Error message")]));

        // Act
        var result = await _sut.Handle(request, () => Task.FromResult(Result.Success()), CancellationToken.None);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Error?.ValidationErrors.Should().ContainKey("property");
        result.Error?.ValidationErrors["property"].Should().Contain("Error message");
    }

    public record TestRequest : IAppBaseRequest;
}
