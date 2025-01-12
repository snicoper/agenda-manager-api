using AgendaManager.Domain.ResourceManagement.Resources;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.Services;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.ResourceManagement.Resources.Services;

public class ResourceManagerDeleteTests
{
    private readonly IResourceRepository _resourceRepository;
    private readonly ICanDeleteResourcePolicy _canDeleteResourcePolicy;
    private readonly ResourceManager _sut;

    public ResourceManagerDeleteTests()
    {
        _resourceRepository = Substitute.For<IResourceRepository>();
        _canDeleteResourcePolicy = Substitute.For<ICanDeleteResourcePolicy>();

        _sut = new ResourceManager(_resourceRepository, _canDeleteResourcePolicy);
    }

    [Fact]
    public async Task DeleteResource_ShouldDeleteResource_WhenResourceExists()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();
        SetupResourceRepositoryGetById(resource);
        SetupCalendarResourcePolicyCanDelete(true);

        // Act
        var result = await _sut.DeleteResourceAsync(resource.Id, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteResource_ShouldFailure_WhenResourceNotExists()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();
        SetupResourceRepositoryGetById(null);

        // Act
        var result = await _sut.DeleteResourceAsync(resource.Id, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteResource_ShouldFailure_WhenResourceCannotBeDeleted()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();
        SetupResourceRepositoryGetById(resource);
        SetupCalendarResourcePolicyCanDelete(false);

        // Act
        var result = await _sut.DeleteResourceAsync(resource.Id, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    private void SetupResourceRepositoryGetById(Resource? resource)
    {
        _resourceRepository.GetByIdAsync(
                Arg.Any<ResourceId>(),
                Arg.Any<CancellationToken>())
            .Returns(resource);
    }

    private void SetupCalendarResourcePolicyCanDelete(bool canDelete)
    {
        _canDeleteResourcePolicy.CanDeleteAsync(
                Arg.Any<ResourceId>(),
                Arg.Any<CancellationToken>())
            .Returns(canDelete);
    }
}
