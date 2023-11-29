using System.Text.Json;
using HexagonalArch.Adapter.Persistence.Entities;
using HexagonalArch.Application.Events;
using HexagonalArch.Application.Providers;
using HexagonalArch.Application.Services;
using Microsoft.Extensions.Logging;

namespace HexagonalArch.Adapter.Persistence.Test;

public class OutBoxMessageServiceTest : IClassFixture<HexagonalArchDbContextFixture>
{
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly HexagonalArchDbContext _dbContext;
    private readonly Mock<IGuidProvider> _guidProviderMock;
    private readonly IOutBoxMessageService _sut;
    private readonly CancellationToken _cancellationToken = new CancellationTokenSource().Token;

    public OutBoxMessageServiceTest(HexagonalArchDbContextFixture fixture)
    {
        _guidProviderMock = new Mock<IGuidProvider>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        Mock<ILogger<OutBoxMessageService>> loggerMock = new();
        _dbContext = fixture.DbContext;

        _sut = new OutBoxMessageService(
            _dateTimeProviderMock.Object,
            _guidProviderMock.Object,
            _dbContext,
            loggerMock.Object);
    }

    [Fact]
    public async Task AddIntegrationEventAsync_WhenAnIntegrationEventIsPassedIn_ItShouldSavedItAsPending()
    {
        // Arrange
        const int Zero = 0;
        var expectedId = Guid.NewGuid();
        var now = new DateTime(2030, 1, 2);
        var integrationEvent = new IntegrationEventTest(Guid.NewGuid(), "Name");

        _guidProviderMock.Setup(x => x.NewId()).Returns(expectedId);
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(now);

        // Act
        var newId = await _sut.AddIntegrationEventAsync(integrationEvent, _cancellationToken);

        // Assert
        var expectedType = integrationEvent.GetType();
        var message = _dbContext.OutBoxMessages.FirstOrDefault(x => x.Id == newId)!;

        Assert.Equal(expectedId, newId);
        Assert.Equal(Zero, message.Attempts);
        Assert.Equal(OutBoxMessageStatus.Pending, message.Status);
        Assert.Equal(expectedType.AssemblyQualifiedName, message.Type);
    }

    [Fact]
    public async Task AddIntegrationEventAsync_WhenEventTypeIsNull_ShouldThrowsAnInvalidOperationException()
    {
        // Arrange
        IIntegrationEvent integrationEvent = null!;
        _guidProviderMock.Setup(x => x.NewId()).Returns(Guid.NewGuid);
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(DateTime.Now);

        // Act 
        Func<Task> testCode = () => _sut.AddIntegrationEventAsync(integrationEvent, _cancellationToken);

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(testCode);
        Assert.Equivalent("invalid type", exception.Message);
    }

    [Fact]
    public async Task
        MarkIntegrationEventAsInProgressAsync_WhenAnIntegrationEventIsPassedIn_ItShouldSavedItAsInProgress()
    {
        // Arrange
        var expectedId = Guid.NewGuid();
        var now = new DateTime(2030, 1, 2);
        var integrationEvent = new IntegrationEventTest(Guid.NewGuid(), "Name");

        _guidProviderMock.Setup(x => x.NewId()).Returns(expectedId);
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(now);

        var id = await _sut.AddIntegrationEventAsync(integrationEvent, _cancellationToken);

        // Act
        await _sut.MarkIntegrationEventAsInProgressAsync(id, _cancellationToken);

        // Assert
        var expectedType = integrationEvent.GetType();
        var message = _dbContext.OutBoxMessages.FirstOrDefault(x => x.Id == id)!;
        Assert.Equal(expectedId, id);
        Assert.Equal(1, message.Attempts);
        Assert.Equal(OutBoxMessageStatus.InProgress, message.Status);
        Assert.Equal(expectedType.AssemblyQualifiedName, message.Type);
    }

    [Theory]
    [InlineData(OutBoxMessageStatus.InProgress)]
    [InlineData(OutBoxMessageStatus.Success)]
    public async Task
        MarkIntegrationEventAsInProgressAsync_WhenItsStatusIsNotTheProperOne_ShouldThrowAnException(
            OutBoxMessageStatus status)
    {
        // Arrange
        var now = new DateTime(2030, 1, 2);
        var integrationEvent = new IntegrationEventTest(Guid.NewGuid(), "Name");

        var outBoxMessage = new OutBoxMessage(
            Guid.NewGuid(),
            integrationEvent.GetType().AssemblyQualifiedName!,
            JsonSerializer.Serialize(integrationEvent),
            now)
        { Status = status };

        await _dbContext.OutBoxMessages.AddAsync(outBoxMessage, _cancellationToken);
        await _dbContext.SaveChangesAsync(_cancellationToken);

        // Act
        var testCode = () => _sut.MarkIntegrationEventAsInProgressAsync(outBoxMessage.Id, _cancellationToken);

        // Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(testCode);
        Assert.StartsWith("You can not mark this event as InProgress due to its current status", exception.Message);
    }
}
public record IntegrationEventTest(Guid EventId, string Name) : IIntegrationEvent;
