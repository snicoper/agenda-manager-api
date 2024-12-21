using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Policies;
using AgendaManager.TestCommon.Constants;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Users.Services;

public class EmailUniquenessPolicyTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly EmailUniquenessPolicy _sut;

    public EmailUniquenessPolicyTests()
    {
        _sut = new EmailUniquenessPolicy(_userRepository);
    }

    [Fact]
    public async Task EmailUniquenessChecker_IsUnique_ShouldReturnSuccess_WhenEmailIsNotInUse()
    {
        // Arrange
        _userRepository.ExistsEmailAsync(Arg.Any<EmailAddress>(), Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _sut.IsUnique(UserConstants.UserBob.Email);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task EmailUniquenessChecker_IsUnique_ShouldReturnFailure_WhenEmailIsInUse()
    {
        // Arrange
        _userRepository.ExistsEmailAsync(Arg.Any<EmailAddress>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _sut.IsUnique(UserConstants.UserBob.Email);

        // Assert
        result.Should().BeFalse();
    }
}
