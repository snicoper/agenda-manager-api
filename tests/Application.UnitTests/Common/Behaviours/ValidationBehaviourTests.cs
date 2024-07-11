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
    private readonly ValidationBehaviour<IAppBaseRequest, Result> _sut;
    private readonly IValidator<IAppBaseRequest> _validator;

    public ValidationBehaviourTests()
    {
        _logger = Substitute.For<ILogger<Result>>();
        _validator = Substitute.For<IValidator<IAppBaseRequest>>();
        _sut = new ValidationBehaviour<IAppBaseRequest, Result>(_logger, _validator);
    }

    [Fact]
    public async Task Handle_ShouldCallNext_WhenValidatorIsNull()
    {
        // Arrange
        TestRequest request = new();
        var next = Substitute.For<RequestHandlerDelegate<Result>>();
        var sut = new ValidationBehaviour<TestRequest, Result>(_logger);

        // Act
        await sut.Handle(request, next, CancellationToken.None);

        // Assert
        await next.Received(1).Invoke();
    }

    [Fact]
    public async Task Handle_ShouldReturnResult_WhenValidationSucceeds()
    {
        // Arrange
        TestRequest request = new();
        _validator.ValidateAsync(request, CancellationToken.None)
            .Returns(new ValidationResult());

        // Act
        var result = await _sut.Handle(request, () => Task.FromResult(Result.Success()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Error?.HasErrors.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldReturnErrorResult_WhenValidationFails()
    {
        // Arrange
        TestRequest request = new();
        var errors = new ValidationFailure("Property", "Error message");

        _validator.ValidateAsync(request, CancellationToken.None)
            .Returns(new ValidationResult([errors]));

        // Act
        var result = await _sut.Handle(request, () => Task.FromResult(Result.Success()), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error?.ToDictionary().Should().ContainKey("property");
        result.Error?.ToDictionary()["property"].Should().Contain("Error message");
    }

    private record TestRequest : IAppBaseRequest;
}
