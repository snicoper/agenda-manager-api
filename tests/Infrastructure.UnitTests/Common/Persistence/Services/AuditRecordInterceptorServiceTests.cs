using AgendaManager.Application.Common.Interfaces.Clock;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.AuditRecords;
using AgendaManager.Domain.Users;
using AgendaManager.Infrastructure.Common.Persistence;
using AgendaManager.Infrastructure.Common.Persistence.Services;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace AgendaManager.Infrastructure.UnitTests.Common.Persistence.Services;

public class AuditRecordInterceptorServiceTests
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<AuditRecordInterceptorService> _logger;
    private readonly AuditRecordInterceptorService _sut;

    public AuditRecordInterceptorServiceTests()
    {
        _currentUserProvider = Substitute.For<ICurrentUserProvider>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _logger = Substitute.For<ILogger<AuditRecordInterceptorService>>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _dbContext = new AppDbContext(options);

        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();

        _sut = new AuditRecordInterceptorService(
            _currentUserProvider,
            _dateTimeProvider,
            _logger);
    }

    [Fact]
    public async Task RecordAuditEntries_ShouldCreateAuditRecord_WhenUserIsModified()
    {
        // Arrange
        var currentDate = DateTimeOffset.UtcNow;
        var user = UserFactory.CreateUserAlice();
        var currentUser = new CurrentUser(user.Id, [], []);

        user.LastModifiedBy = "John";
        user.CreatedBy = "John";

        _dateTimeProvider.UtcNow.Returns(currentDate);
        _currentUserProvider.GetCurrentUser().Returns(currentUser);

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        user.UpdateActiveState(false);
        _dbContext.Users.Update(user);

        // Act
        _sut.RecordAuditEntries<User>(_dbContext, nameof(User.Id), [nameof(User.Active)]);
        await _dbContext.SaveChangesAsync();

        // Assert
        var auditRecords = _dbContext.Set<AuditRecord>().ToList();
        auditRecords.Should().HaveCount(1);
    }

    [Fact]
    public async Task RecordAuditEntries_ShouldNotCreateAuditRecord_WhenUserIsNotModified()
    {
        // Arrange
        var currentDate = DateTimeOffset.UtcNow;
        var user = UserFactory.CreateUserAlice();
        var currentUser = new CurrentUser(user.Id, [], []);

        user.LastModifiedBy = "John";
        user.CreatedBy = "John";

        _dateTimeProvider.UtcNow.Returns(currentDate);
        _currentUserProvider.GetCurrentUser().Returns(currentUser);

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Emulate no changes to the user object.
        user.UpdateActiveState(true);
        _dbContext.Users.Update(user);

        // Act
        _sut.RecordAuditEntries<User>(_dbContext, nameof(User.Id), [nameof(User.Active)]);
        await _dbContext.SaveChangesAsync();

        // Assert
        var auditRecords = _dbContext.Set<AuditRecord>().ToList();
        auditRecords.Should().HaveCount(0);
    }

    [Fact]
    public async Task RecordAuditEntries_ShouldNotCreateAuditRecord_WhenAuditablePropertyIsEmpty()
    {
        // Arrange
        var currentDate = DateTimeOffset.UtcNow;
        var user = UserFactory.CreateUserAlice();
        var currentUser = new CurrentUser(user.Id, [], []);

        user.LastModifiedBy = "John";
        user.CreatedBy = "John";

        _dateTimeProvider.UtcNow.Returns(currentDate);
        _currentUserProvider.GetCurrentUser().Returns(currentUser);

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Emulate no changes to the user object.
        user.UpdateActiveState(true);
        _dbContext.Users.Update(user);

        // Act
        _sut.RecordAuditEntries<User>(_dbContext, nameof(User.Id), []);
        await _dbContext.SaveChangesAsync();

        // Assert
        var auditRecords = _dbContext.Set<AuditRecord>().ToList();
        auditRecords.Should().HaveCount(0);
    }

    [Fact]
    public async Task RecordAuditEntries_ShouldLogError_WhenExceptionOccurs()
    {
        // Arrange
        var currentDate = DateTimeOffset.UtcNow;
        var user = UserFactory.CreateUserAlice();
        var currentUser = new CurrentUser(user.Id, [], []);
        var propertyNameNotExists = "NotExists";

        user.LastModifiedBy = "John";
        user.CreatedBy = "John";

        _dateTimeProvider.UtcNow.Returns(currentDate);
        _currentUserProvider.GetCurrentUser().Returns(currentUser);

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Emulate no changes to the user object.
        user.UpdateActiveState(true);
        _dbContext.Users.Update(user);

        // Act
        // Simulate an exception by trying to access a non-existent property.
        _sut.RecordAuditEntries<User>(_dbContext, nameof(User.Id), [propertyNameNotExists]);
        await _dbContext.SaveChangesAsync();

        // Assert
        _logger.Received().Log(
            LogLevel.Error,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }
}
