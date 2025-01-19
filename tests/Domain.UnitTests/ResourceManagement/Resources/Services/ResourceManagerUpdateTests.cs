using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources;
using AgendaManager.Domain.ResourceManagement.Resources.Errors;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.Services;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.ResourceManagement.Resources.Services;

public class ResourceManagerUpdateTests
{
    private readonly Resource _resourceUpdated =
        ResourceFactory.CreateResource(name: "new name", description: "new description");

    private readonly IResourceRepository _resourceRepository;
    private readonly ResourceManager _sut;

    public ResourceManagerUpdateTests()
    {
        _resourceRepository = Substitute.For<IResourceRepository>();
        var canDeleteResourcePolicy = Substitute.For<ICanDeleteResourcePolicy>();

        _sut = new ResourceManager(_resourceRepository, canDeleteResourcePolicy);
    }

    [Fact]
    public async Task UpdateResource_ShouldUpdate_WhenResourceAreProvided()
    {
        // Arrange
        SetupGetByIdResourceRepository(_resourceUpdated);
        SetupExistsByNameResourceRepository(false);

        // Act
        var result = await UpdateResourceAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(_resourceUpdated);
    }

    [Fact]
    public async Task UpdateResource_ShouldFailure_WhenResourceNotFound()
    {
        // Arrange
        SetupGetByIdResourceRepository(null);

        // Act
        var result = await UpdateResourceAsync();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(ResourceErrors.NotFound.FirstError());
    }

    [Fact]
    public async Task UpdateResource_ShouldFailure_WhenExistsByName()
    {
        // Arrange
        SetupGetByIdResourceRepository(_resourceUpdated);
        SetupExistsByNameResourceRepository(true);

        // Act
        var result = await UpdateResourceAsync();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(ResourceErrors.NameAlreadyExists.FirstError());
    }

    private void SetupGetByIdResourceRepository(Resource? returnValue)
    {
        _resourceRepository.GetByIdAsync(Arg.Any<ResourceId>(), Arg.Any<CancellationToken>())
            .Returns(returnValue);
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

    private async Task<Result<Resource>> UpdateResourceAsync(string? newName = null, string? newDescription = null)
    {
        var result = await _sut.UpdateResourceAsync(
            resourceId: ResourceId.Create(),
            name: newName ?? "Name",
            description: newDescription ?? "Description",
            colorScheme: ColorScheme.From("#FFFFFF", "#000000"),
            cancellationToken: CancellationToken.None);

        return result;
    }
}
