using AgendaManager.Application.Auth.Commands.Login;
using AgendaManager.Application.Common.Constants;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Extensions;
using AgendaManager.Domain.Common.Responses;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Application.UnitTests.Auth.Commands.Login;

public class LoginHandlerTests
{
    private readonly IAuthService _authService;
    private readonly LoginHandler _sut;

    public LoginHandlerTests()
    {
        _authService = Substitute.For<IAuthService>();
        _sut = new LoginHandler(_authService);
    }

    [Fact]
    public async Task LoginHandler_ReturnsUnauthorized()
    {
        // Arrange
        var loginCommand = CreateLoginCommand();

        _authService
            .LoginAsync(Arg.Any<string>(), Arg.Any<string>())
            .Returns(Error.Unauthorized<TokenResponse>());

        // Act
        var handleResult = await _sut.Handle(loginCommand, default);

        // Assert
        handleResult.ErrorType.Should().Be(ErrorType.Unauthorized);
    }

    [Fact]
    public async Task LoginHandler_ReturnsBadRequest()
    {
        // Arrange
        var loginCommand = CreateLoginCommand();
        var validationErrorKey = ValidationErrors.NonFieldErrors.ToLowerFirstLetter();

        _authService
            .LoginAsync(Arg.Any<string>(), Arg.Any<string>())
            .Returns(Error.Validation<TokenResponse>(validationErrorKey, string.Empty));

        // Act
        var handleResult = await _sut.Handle(loginCommand, default);

        // Assert
        handleResult.ErrorType.Should().Be(ErrorType.ValidationError);
        handleResult.Error?.ValidationErrors.First().Key.Should().Be(validationErrorKey);
    }

    [Fact]
    public async Task LoginHandler_ReturnsResultSuccess()
    {
        // Arrange
        var loginCommand = CreateLoginCommand();
        var result = Result.Success(new TokenResponse("accessToken", "refreshToken"));

        _authService.LoginAsync(Arg.Is(loginCommand.Email), Arg.Is(loginCommand.Password)).Returns(result);

        // Act
        var handleResult = await _sut.Handle(loginCommand, default);

        // Assert
        handleResult.Should().BeEquivalentTo(result);
        handleResult.ErrorType.Should().Be(ErrorType.None);
    }

    private static LoginCommand CreateLoginCommand()
    {
        return new LoginCommand("email@example.com", "password");
    }
}
