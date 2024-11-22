using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarConfigurations;

public class CalendarConfigurationCreateTests
{
    [Fact]
    public void Create_ShouldSuccess_WhenValidParametersProvided()
    {
        // Act
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration();

        // Assert
        configuration.Should().NotBeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void Create_ShouldRaiseException_WhenInvalidCategoryProvided(int categoryLength)
    {
        // Act
        var category = new string('*', categoryLength);
        var action = () => CalendarConfigurationFactory.CreateCalendarConfiguration(category: category);

        // Assert
        action.Should().Throw<CalendarConfigurationDomainException>()
            .WithMessage("The category must be between 0 and 100 characters.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void Create_ShouldRaiseException_WhenInvalidSelectedKeyProvided(int selectedKeyLength)
    {
        // Act
        var selectedKey = new string('*', selectedKeyLength);
        var action = () => CalendarConfigurationFactory.CreateCalendarConfiguration(selectedKey: selectedKey);

        // Assert
        action.Should().Throw<CalendarConfigurationDomainException>()
            .WithMessage("The selected key must be between 0 and 100 characters.");
    }

    [Fact]
    public void Create_ShouldRaiseException_WhenInvalidConfigurationProvided()
    {
        // Act
        var action = () => CalendarConfigurationFactory.CreateCalendarConfiguration(
            category: "CategoryNotExist",
            selectedKey: CalendarConfigurationKeys.Appointments.ConfirmationOptions.RequireConfirmation);

        // Assert
        action.Should().Throw<CalendarConfigurationDomainException>()
            .WithMessage("Invalid configuration combination: CategoryNotExist - RequireConfirmation");
    }
}
