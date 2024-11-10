using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users.Aggregates.Services;

public class EmailUniquenessCheckerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly EmailUniquenessChecker _sut;

    public EmailUniquenessCheckerTests()
    {
        _sut = new EmailUniquenessChecker(_userRepository);
    }

    [Fact]
    public async Task EmailUniquenessChecker_IsUnique_ShouldReturnSuccess_WhenEmailIsNotInUse()
    {
        // Arrange
        var email = EmailAddress.From("email@email.com");
        _userRepository.EmailExistsAsync(Arg.Any<EmailAddress>(), Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _sut.IsUnique(email);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task EmailUniquenessChecker_IsUnique_ShouldReturnFailure_WhenEmailIsInUse()
    {
        // Arrange
        var email = EmailAddress.From("email@email.com");
        _userRepository.EmailExistsAsync(Arg.Any<EmailAddress>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _sut.IsUnique(email);

        // Assert
        result.Should().BeFalse();
    }
}
