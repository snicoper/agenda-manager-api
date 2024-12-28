using AgendaManager.Domain.ResourceManagement.Resources;
using AgendaManager.Domain.Services;
using AgendaManager.Domain.Services.Errors;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Services.Policies;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using NSubstitute;

namespace AgendaManager.Domain.UnitTests.Services.Policies;

public class ServiceRequirementsPolicyTests
{
    private readonly IServiceRepository _serviceRepository;
    private readonly ServiceRequirementsPolicy _sut;

    public ServiceRequirementsPolicyTests()
    {
        _serviceRepository = Substitute.For<IServiceRepository>();
        _sut = new ServiceRequirementsPolicy(_serviceRepository);
    }

    [Fact]
    public async Task IsSatisfiedBy_ShouldReturnSuccess_WhenValidResourcesAreProvided()
    {
        // Arrange
        var resourceType = ResourceTypeFactory.CreateResourceType();
        var service = ServiceFactory.CreateService();
        service.AddResourceType(resourceType);

        List<Resource> resources = [ResourceFactory.CreateResource(resourceTypeId: resourceType.Id)];

        _serviceRepository.GetByIdWithResourceTypesAsync(Arg.Any<ServiceId>(), Arg.Any<CancellationToken>())
            .Returns(service);

        // Act
        var result = await _sut.IsSatisfiedByAsync(service.Id, resources, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task IsSatisfiedBy_ShouldReturnFailure_WhenServiceNotExists()
    {
        // Arrange
        var resourceType = ResourceTypeFactory.CreateResourceType();
        var service = ServiceFactory.CreateService();
        service.AddResourceType(resourceType);

        List<Resource> resources = [ResourceFactory.CreateResource(resourceTypeId: resourceType.Id)];
        _serviceRepository.GetByIdWithResourceTypesAsync(Arg.Any<ServiceId>(), Arg.Any<CancellationToken>())
            .Returns((Service?)null);

        // Act
        var result = await _sut.IsSatisfiedByAsync(ServiceId.Create(), resources, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(ServiceErrors.ServiceNotFound.FirstError());
    }

    [Fact]
    public async Task IsSatisfiedBy_ShouldReturnFailure_WhenResourcesNotProvided()
    {
        // Arrange
        _serviceRepository.GetByIdWithResourceTypesAsync(Arg.Any<ServiceId>(), Arg.Any<CancellationToken>())
            .Returns((Service?)null);

        // Act
        var result = await _sut.IsSatisfiedByAsync(ServiceId.Create(), [], CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(ServiceErrors.ResourceRequirementsMismatch.FirstError());
    }

    [Fact]
    public async Task IsSatisfiedBy_ShouldReturnFailure_WhenInvalidResourcesAreProvided()
    {
        // Arrange
        var resourceType = ResourceTypeFactory.CreateResourceType();
        var service = ServiceFactory.CreateService();
        service.AddResourceType(resourceType);

        List<Resource> resources = [ResourceFactory.CreateResource()];

        _serviceRepository.GetByIdWithResourceTypesAsync(Arg.Any<ServiceId>(), Arg.Any<CancellationToken>())
            .Returns(service);

        // Act
        var result = await _sut.IsSatisfiedByAsync(service.Id, resources, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(ServiceErrors.ResourceRequirementsMismatch.FirstError());
    }

    [Fact]
    public async Task IsSatisfiedBy_ShouldReturnFailure_WhenMoreResourcesAreProvided()
    {
        // Arrange
        var resourceType = ResourceTypeFactory.CreateResourceType();
        var service = ServiceFactory.CreateService();
        service.AddResourceType(resourceType);

        List<Resource> resources =
        [
            ResourceFactory.CreateResource(resourceTypeId: resourceType.Id),
            ResourceFactory.CreateResource(resourceTypeId: resourceType.Id)
        ];

        _serviceRepository.GetByIdWithResourceTypesAsync(Arg.Any<ServiceId>(), Arg.Any<CancellationToken>())
            .Returns(service);

        // Act
        var result = await _sut.IsSatisfiedByAsync(service.Id, resources, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(ServiceErrors.ResourceRequirementsMismatch.FirstError());
    }
}
