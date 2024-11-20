using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceTypes;
using AgendaManager.Domain.ResourceTypes.Errors;
using AgendaManager.Domain.ResourceTypes.Events;
using AgendaManager.Domain.ResourceTypes.Interfaces;
using AgendaManager.Domain.ResourceTypes.Services;
using AgendaManager.Domain.ResourceTypes.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.ResourceTypes.Managers;

public class ResourceTypeManagerUpdateTests
{
    private const string _newName = "New Name";
    private const string _newDescription = "New Description";
    private readonly ResourceType _resourceType = ResourceTypeFactory.CreateResourceType();

    private readonly IResourceTypeRepository _resourceTypeRepository;
    private readonly ResourceTypeManager _sut;

    public ResourceTypeManagerUpdateTests()
    {
        _resourceTypeRepository = Substitute.For<IResourceTypeRepository>();
        var roleRepository = Substitute.For<IRoleRepository>();

        _sut = new ResourceTypeManager(_resourceTypeRepository, roleRepository);
    }

    [Fact]
    public async Task Update_ShouldRaiseEvent_WhenResourceTypeIsValid()
    {
        // Arrange
        SetupNameExistsResourceTypeRepository(false);
        SetupDescriptionExistsResourceTypeRepository(false);
        SetupGetByIdAsyncResourceTypeRepository(_resourceType);

        // Act
        var result = await UpdateResourceTypeAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value?.DomainEvents.Should().Contain(x => x is ResourceTypeUpdatedDomainEvent);
    }

    [Fact]
    public async Task Update_ShouldNotRaiseEvent_WhenResourceTypeIdNotExists()
    {
        // Arrange
        SetupNameExistsResourceTypeRepository(false);
        SetupDescriptionExistsResourceTypeRepository(false);
        SetupGetByIdAsyncResourceTypeRepository();

        // Act
        var result = await UpdateResourceTypeAsync();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Value?.DomainEvents.Should().NotContain(x => x is ResourceTypeUpdatedDomainEvent);
    }

    [Fact]
    public async Task Update_ShouldFailure_WhenNameExists()
    {
        // Arrange
        var errorExpected = ResourceTypeErrors.NameAlreadyExists.FirstError();
        SetupNameExistsResourceTypeRepository(true);

        // Act
        var result = await UpdateResourceTypeAsync();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(errorExpected);
    }

    [Fact]
    public async Task Update_ShouldFailure_WhenDescriptionExists()
    {
        // Arrange
        var errorExpected = ResourceTypeErrors.DescriptionExists.FirstError();
        SetupNameExistsResourceTypeRepository(false);
        SetupDescriptionExistsResourceTypeRepository(true);

        // Act
        var result = await UpdateResourceTypeAsync();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(errorExpected);
    }

    private void SetupGetByIdAsyncResourceTypeRepository(ResourceType? returnValue = null)
    {
        _resourceTypeRepository.GetByIdAsync(Arg.Any<ResourceTypeId>(), Arg.Any<CancellationToken>())
            .Returns(returnValue);
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

    private async Task<Result<ResourceType>> UpdateResourceTypeAsync(
        string? newName = null,
        string? newDescription = null)
    {
        var result = await _sut.UpdateResourceTypeAsync(
            resourceTypeId: _resourceType.Id,
            name: newName ?? _newName,
            description: newDescription ?? _newDescription,
            cancellationToken: CancellationToken.None);

        return result;
    }
}
