using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.Services;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.ResourceManagement.Resources.Services;

public class ResourceManagerCreateTests
{
    private readonly IResourceRepository _resourceRepository;
    private readonly ResourceManager _sut;

    public ResourceManagerCreateTests()
    {
        _resourceRepository = Substitute.For<IResourceRepository>();
        var canDeleteResourcePolicy = Substitute.For<ICanDeleteResourcePolicy>();

        _sut = new ResourceManager(_resourceRepository, canDeleteResourcePolicy);
    }

    [Fact]
    public async Task CreateResource_ShouldCreateResource_WhenResourceAreProvided()
    {
        // Arrange
        SetupExistsByNameResourceRepository(false);

        // Act
        var result = await CreateResourceAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Name.Should().Be("Name");
        result.Value.Description.Should().Be("Description");
        result.Value.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task CreateResource_ShouldFailure_WhenNameAlreadyExists()
    {
        // Arrange
        SetupExistsByNameResourceRepository(true);

        // Act
        var result = await CreateResourceAsync();

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    private void SetupExistsByNameResourceRepository(bool returnValue)
    {
        _resourceRepository.ExistsByNameAsync(
                Arg.Any<CalendarId>(),
                Arg.Any<ResourceId>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }

    private async Task<Result<Resource>> CreateResourceAsync(
        string? newName = null,
        string? newDescription = null,
        bool? isActive = null)
    {
        var result = await _sut.CreateResourceAsync(
            resourceId: ResourceId.Create(),
            userId: UserId.Create(),
            calendarId: CalendarId.Create(),
            typeId: ResourceTypeId.Create(),
            name: newName ?? "Name",
            description: newDescription ?? "Description",
            colorScheme: ColorScheme.From("#FFFFFF", "#000000"),
            isActive: isActive ?? true,
            cancellationToken: CancellationToken.None);

        return result;
    }
}
