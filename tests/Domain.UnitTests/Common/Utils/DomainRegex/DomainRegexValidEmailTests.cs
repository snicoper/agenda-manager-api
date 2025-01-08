using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.Utils.DomainRegex;

public class DomainRegexValidEmailTests
{
    [Theory]
    [InlineData("usuario.ejemplo@dominio.com")]
    [InlineData("usuario-ejemplo@dominio.co.uk")]
    [InlineData("usuario_nombre@dominio.org")]
    [InlineData("usuario.nombre@subdominio.com")]
    public void ValidEmail_ShouldReturnTrue_WhenValidEmailIsProvided(string email)
    {
        // Act
        var result = Domain.Common.Utils.DomainRegex.ValidEmail().IsMatch(email);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("usuario@dominio")]
    [InlineData("usuario@.com")]
    [InlineData("usuario@dominio.")]
    [InlineData("usuario@dominio.c")]
    [InlineData("usuario@dominio..com")]
    [InlineData("usuario@dominio.123")]
    [InlineData("usuario.@dominio.com")]
    [InlineData(".usuario@dominio.com")]
    [InlineData("")]
    public void InvalidEmail_ShouldReturnFalse_WhenInvalidEmailIsProvided(string email)
    {
        // Act
        var result = Domain.Common.Utils.DomainRegex.ValidEmail().IsMatch(email);

        // Assert
        result.Should().BeFalse();
    }
}
