﻿using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.ResourceTypes;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Errors;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Events;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Interfaces;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Services;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.ResourceManagement.ResourceTypes.Managers;

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
        var canDeleteResourceTypePolicy = Substitute.For<ICanDeleteResourceTypePolicy>();

        _sut = new ResourceTypeManager(_resourceTypeRepository, canDeleteResourceTypePolicy);
    }

    [Fact]
    public async Task Update_ShouldRaiseEvent_WhenResourceTypeIsValid()
    {
        // Arrange
        SetupExistsByNameResourceTypeRepository(false);
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
        SetupExistsByNameResourceTypeRepository(false);
        SetupGetByIdAsyncResourceTypeRepository();

        // Act
        var result = await UpdateResourceTypeAsync();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Value?.DomainEvents.Should().NotContain(x => x is ResourceTypeUpdatedDomainEvent);
    }

    [Fact]
    public async Task Update_ShouldFailure_WhenExistsByName()
    {
        // Arrange
        var errorExpected = ResourceTypeErrors.NameAlreadyExists.FirstError();
        SetupExistsByNameResourceTypeRepository(true);

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

    private void SetupExistsByNameResourceTypeRepository(bool returnValue)
    {
        _resourceTypeRepository.ExistsByNameAsync(
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
