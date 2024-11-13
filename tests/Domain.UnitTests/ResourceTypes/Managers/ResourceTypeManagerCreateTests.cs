using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceTypes;
using AgendaManager.Domain.ResourceTypes.Errors;
using AgendaManager.Domain.ResourceTypes.Interfaces;
using AgendaManager.Domain.ResourceTypes.Services;
using AgendaManager.Domain.ResourceTypes.ValueObjects;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;
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
        SetupNameExistsResourceTypeRepository(false);
        SetupDescriptionExistsResourceTypeRepository(false);

        // Act
        var resourceType = await GetResourceTypeAsync();

        // Assert
        resourceType.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Create_ShouldSuccess_WhenRoleIdIsProvided()
    {
        // Arrange
        SetupNameExistsResourceTypeRepository(false);
        SetupDescriptionExistsResourceTypeRepository(false);
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
    public async Task Create_ShouldErrorNameExists_WhenNameInResourceTypeAlreadyExists()
    {
        // Arrange
        var errorExpected = ResourceTypeErrors.NameAlreadyExists.FirstError();
        SetupNameExistsResourceTypeRepository(true);

        // Act
        var resourceType = await GetResourceTypeAsync();

        // Assert
        resourceType.IsFailure.Should().BeTrue();
        resourceType.Error?.FirstError().Should().Be(errorExpected);
    }

    [Fact]
    public async Task Create_ShouldErrorDescriptionExists_WhenDescriptionInResourceTypeAlreadyExists()
    {
        // Arrange
        var errorExpected = ResourceTypeErrors.DescriptionExists.FirstError();
        SetupDescriptionExistsResourceTypeRepository(true);

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

    private void SetupNameExistsResourceTypeRepository(bool returnValue)
    {
        _resourceTypeRepository.NameExistsAsync(
                Arg.Any<ResourceTypeId>(),
                Arg.Any<string>(),
                Arg.Any<CancellationToken>())
            .Returns(returnValue);
    }

    private void SetupDescriptionExistsResourceTypeRepository(bool returnValue)
    {
        _resourceTypeRepository.DescriptionExistsAsync(
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
