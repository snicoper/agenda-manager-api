using AgendaManager.Domain.ResourceManagement.ResourceTypes;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Interfaces;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Services;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.ResourceManagement.ResourceTypes.Managers;

public class ResourceTypeManagerDeleteTests
{
    private readonly IResourceTypeRepository _resourceTypeRepository;
    private readonly ICanDeleteResourceTypePolicy _canDeleteResourceTypePolicy;
    private readonly ResourceTypeManager _sut;

    public ResourceTypeManagerDeleteTests()
    {
        _resourceTypeRepository = Substitute.For<IResourceTypeRepository>();
        _canDeleteResourceTypePolicy = Substitute.For<ICanDeleteResourceTypePolicy>();

        _sut = new ResourceTypeManager(_resourceTypeRepository, _canDeleteResourceTypePolicy);
    }

    [Fact]
    public async Task Delete_ShouldSuccess_WhenResourceTypeIsValid()
    {
        // Arrange
        var resourceType = ResourceTypeFactory.CreateResourceType();
        SetupGetByIdAsyncResourceTypeRepository(resourceType);
        SetupCanDeleteResourceTypePolicy(true);

        // Act
        var result = await _sut.DeleteResourceTypeAsync(resourceType.Id, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_ShouldFailure_WhenResourceTypeNotExists()
    {
        // Arrange
        SetupGetByIdAsyncResourceTypeRepository(null);

        // Act
        var result = await _sut.DeleteResourceTypeAsync(ResourceTypeId.Create(), CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_ShouldFailure_WhenResourceTypeCannotBeDeleted()
    {
        // Arrange
        var resourceType = ResourceTypeFactory.CreateResourceType();
        SetupGetByIdAsyncResourceTypeRepository(resourceType);
        SetupCanDeleteResourceTypePolicy(false);

        // Act
        var result = await _sut.DeleteResourceTypeAsync(resourceType.Id, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    private void SetupGetByIdAsyncResourceTypeRepository(ResourceType? resourceType)
    {
        _resourceTypeRepository.GetByIdAsync(Arg.Any<ResourceTypeId>(), Arg.Any<CancellationToken>())
            .Returns(resourceType);
    }

    private void SetupCanDeleteResourceTypePolicy(bool canDelete)
    {
        _canDeleteResourceTypePolicy.CanDeleteAsync(Arg.Any<ResourceType>(), Arg.Any<CancellationToken>())
            .Returns(canDelete);
    }
}
