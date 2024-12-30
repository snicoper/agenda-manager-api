using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.ResourceTypes;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Errors;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Interfaces;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Services;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Shared.Enums;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.ResourceManagement.ResourceTypes.Managers;

public class ResourceTypeManagerCreateTests
{
    private readonly IResourceTypeRepository _resourceTypeRepository;
    private readonly ResourceTypeManager _sut;

    public ResourceTypeManagerCreateTests()
    {
        _resourceTypeRepository = Substitute.For<IResourceTypeRepository>();
        var canDeleteResourceTypePolicy = Substitute.For<ICanDeleteResourceTypePolicy>();

        _sut = new ResourceTypeManager(_resourceTypeRepository, canDeleteResourceTypePolicy);
    }

    [Fact]
    public async Task Create_ShouldSuccess_WhenResourceTypeIsValid()
    {
        // Arrange
        SetupExistsByNameResourceTypeRepository(false);

        // Act
        var resourceType = await GetResourceTypeAsync();

        // Assert
        resourceType.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Create_ShouldSuccess_WhenRoleIdIsProvided()
    {
        // Arrange
        SetupExistsByNameResourceTypeRepository(false);

        // Act
        var resourceType = await _sut.CreateResourceTypeAsync(
            ResourceTypeId.Create(),
            "Name",
            "Description",
            ResourceCategory.Staff,
            CancellationToken.None);

        // Assert
        resourceType.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Create_ShouldErrorExistsByName_WhenNameInResourceTypeAlreadyExists()
    {
        // Arrange
        var errorExpected = ResourceTypeErrors.NameAlreadyExists.FirstError();
        SetupExistsByNameResourceTypeRepository(true);

        // Act
        var resourceType = await GetResourceTypeAsync();

        // Assert
        resourceType.IsFailure.Should().BeTrue();
        resourceType.Error?.FirstError().Should().Be(errorExpected);
    }

    private void SetupExistsByNameResourceTypeRepository(bool returnValue)
    {
        _resourceTypeRepository.ExistsByNameAsync(
                Arg.Any<ResourceTypeId>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }

    private async Task<Result<ResourceType>> GetResourceTypeAsync()
    {
        var resourceType = await _sut.CreateResourceTypeAsync(
            ResourceTypeId.Create(),
            "Name",
            "Description",
            ResourceCategory.Staff,
            CancellationToken.None);

        return resourceType;
    }
}
