using AgendaManager.Domain.Authorization.Errors;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceTypes;
using AgendaManager.Domain.ResourceTypes.Errors;
using AgendaManager.Domain.ResourceTypes.Interfaces;
using AgendaManager.Domain.ResourceTypes.Services;
using AgendaManager.Domain.ResourceTypes.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.ResourceTypes.Managers;

public class ResourceTypeManagerCreateTests
{
    private readonly IResourceTypeRepository _resourceTypeRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ResourceTypeManager _sut;

    public ResourceTypeManagerCreateTests()
    {
        _resourceTypeRepository = Substitute.For<IResourceTypeRepository>();
        _roleRepository = Substitute.For<IRoleRepository>();

        _sut = new ResourceTypeManager(_resourceTypeRepository, _roleRepository);
    }

    [Fact]
    public async Task Create_ShouldSuccess_WhenResourceTypeIsValid()
    {
        // Arrange
        SetupExistsByNameResourceTypeRepository(false);
        SetupExistsByDescriptionResourceTypeRepository(false);

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
        SetupExistsByDescriptionResourceTypeRepository(false);
        SetupExistsByIdRoleRepository(true);

        // Act
        var resourceType = await _sut.CreateResourceTypeAsync(
            ResourceTypeId.Create(),
            "Name",
            "Description",
            CancellationToken.None,
            RoleId.Create());

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

    [Fact]
    public async Task Create_ShouldErrorExistsDescription_WhenInResourceTypeAlreadyExists()
    {
        // Arrange
        var errorExpected = ResourceTypeErrors.DescriptionExists.FirstError();
        SetupExistsByDescriptionResourceTypeRepository(true);

        // Act
        var resourceType = await GetResourceTypeAsync();

        // Assert
        resourceType.IsFailure.Should().BeTrue();
        resourceType.Error?.FirstError().Should().Be(errorExpected);
    }

    [Fact]
    public async Task Create_ShouldErrorRoleNotFound_WhenRoleIdIsNotFound()
    {
        // Arrange
        var errorExpected = RoleErrors.RoleNotFound.FirstError();
        SetupExistsByIdRoleRepository(false);

        // Act
        var resourceType = await _sut.CreateResourceTypeAsync(
            ResourceTypeId.Create(),
            "Name",
            "Description",
            CancellationToken.None,
            RoleId.Create());

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

    private void SetupExistsByDescriptionResourceTypeRepository(bool returnValue)
    {
        _resourceTypeRepository.ExistsByDescriptionAsync(
                Arg.Any<ResourceTypeId>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }

    private void SetupExistsByIdRoleRepository(bool returnValue)
    {
        _roleRepository.ExistsByIdAsync(Arg.Any<RoleId>(), Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }

    private async Task<Result<ResourceType>> GetResourceTypeAsync()
    {
        var resourceType = await _sut.CreateResourceTypeAsync(
            ResourceTypeId.Create(),
            "Name",
            "Description",
            CancellationToken.None);

        return resourceType;
    }
}
