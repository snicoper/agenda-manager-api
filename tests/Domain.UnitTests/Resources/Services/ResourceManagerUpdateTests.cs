using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.ColorScheme;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Resources.Errors;
using AgendaManager.Domain.Resources.Interfaces;
using AgendaManager.Domain.Resources.Services;
using AgendaManager.Domain.Resources.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Resources.Services;

public class ResourceManagerUpdateTests
{
    private readonly Resource _resourceUpdated =
        ResourceFactory.CreateResource(name: "new name", description: "new description");

    private readonly IResourceRepository _resourceRepository;
    private readonly ResourceManager _sut;

    public ResourceManagerUpdateTests()
    {
        _resourceRepository = Substitute.For<IResourceRepository>();

        _sut = new ResourceManager(_resourceRepository);
    }

    [Fact]
    public async Task UpdateResource_ShouldUpdate_WhenResourceAreProvided()
    {
        // Arrange
        SetupGetByIdResourceRepository(_resourceUpdated);
        SetupExistsByNameResourceRepository(false);
        SetupExistsDescriptionResourceRepository(false);

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

    [Fact]
    public async Task UpdateResource_ShouldFailure_WhenDescriptionExists()
    {
        // Arrange
        SetupGetByIdResourceRepository(_resourceUpdated);
        SetupExistsByNameResourceRepository(false);
        SetupExistsDescriptionResourceRepository(true);

        // Act
        var result = await UpdateResourceAsync();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(ResourceErrors.DescriptionAlreadyExists.FirstError());
    }

    private void SetupGetByIdResourceRepository(Resource? returnValue)
    {
        _resourceRepository.GetByIdAsync(Arg.Any<ResourceId>(), Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }

    private void SetupExistsByNameResourceRepository(bool returnValue)
    {
        _resourceRepository.ExistsByNameAsync(Arg.Any<ResourceId>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }

    private void SetupExistsDescriptionResourceRepository(bool returnValue)
    {
        _resourceRepository.ExistsByDescriptionAsync(
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
