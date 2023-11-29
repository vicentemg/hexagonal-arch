using System.Text.Json;
using HexagonalArch.Adapter.Persistence.Entities;
using HexagonalArch.Application.Events;
using HexagonalArch.Application.Providers;
using HexagonalArch.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HexagonalArch.Adapter.Persistence;

public class OutBoxMessageService : IOutBoxMessageService
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly HexagonalArchDbContext _dbContext;
    private readonly IGuidProvider _guidProvider;
    private readonly ILogger<OutBoxMessageService> _logger;

    public OutBoxMessageService(
        IDateTimeProvider dateTimeProvider,
        IGuidProvider guidProvider,
        HexagonalArchDbContext dbContext,
        ILogger<OutBoxMessageService> logger)
    {
        _dateTimeProvider = dateTimeProvider;
        _guidProvider = guidProvider;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Guid> AddIntegrationEventAsync(IIntegrationEvent integrationEvent,
        CancellationToken cancellationToken)
    {
        var id = _guidProvider.NewId();
        var occurredOn = _dateTimeProvider.UtcNow;
        var eventType = integrationEvent?.GetType() ?? throw new InvalidOperationException("invalid type");
        var eventData = JsonSerializer.Serialize(integrationEvent, eventType);
        var outBoxMessage = new OutBoxMessage(id, eventType.AssemblyQualifiedName!, eventData, occurredOn);

        await _dbContext.OutBoxMessages.AddAsync(outBoxMessage, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return id;
    }

    public async Task MarkIntegrationEventAsInProgressAsync(Guid eventId, CancellationToken cancellationToken)
    {
        using var _ = _logger.BeginScope("integration event [{eventId}] is being marked as {OutBoxMessageStatus}s",
            eventId,
            OutBoxMessageStatus.InProgress);

        var message = await GetEventByIdAsync(eventId, cancellationToken);

        const OutBoxMessageStatus availableStatuses = OutBoxMessageStatus.Failed | OutBoxMessageStatus.Pending;
        if (!availableStatuses.HasFlag(message.Status))
        {
            _logger.LogError(
                "You can not mark this event as InProgress due to its current status: {CurrentOutBoxMessageStatus}",
                message.Status);
            throw new InvalidOperationException(
                $"You can not mark this event as InProgress due to its current status: {message.Status}");
        }

        message.Status = OutBoxMessageStatus.InProgress;
        message.Attempts += 1;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkIntegrationEventAsSuccessAsync(Guid eventId, CancellationToken cancellationToken)
    {
        using var _ = _logger.BeginScope("integration event [{eventId}] is being marked as {OutBoxMessageStatus}",
            eventId,
            OutBoxMessageStatus.Success);

        var message = await GetEventByIdAsync(eventId, cancellationToken);

        if (message.Status is not OutBoxMessageStatus.InProgress)
        {
            _logger.LogError(
                "You can not mark this event as Success due to its current status: {CurrentOutBoxMessageStatus}",
                message.Status);
            throw new InvalidOperationException(
                $"You can not mark this event as Success due to its current status: {message.Status}");
        }

        message.Status = OutBoxMessageStatus.Success;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkIntegrationEventAsFailedAsync(Guid eventId, CancellationToken cancellationToken)
    {
        using var _ = _logger.BeginScope("integration event [{eventId}] is being marked as {OutBoxMessageStatus}",
            eventId,
            OutBoxMessageStatus.Failed);

        var message = await GetEventByIdAsync(eventId, cancellationToken);

        if (message.Status is not OutBoxMessageStatus.InProgress)
        {
            _logger.LogError(
                "You can not mark this event as Failed due to its current status: {CurrentOutBoxMessageStatus}",
                message.Status);
            throw new InvalidOperationException(
                $"You can not mark this event as Failed due to its current status: {message.Status}");
        }

        message.Status = OutBoxMessageStatus.Failed;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<OutBoxMessage> GetEventByIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var message = await _dbContext.OutBoxMessages.FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

        if (message is not null) return message;

        _logger.LogError("That event id {eventId} does not exist in the Data Base", eventId);
        throw new InvalidOperationException($"That event id {eventId} does not exist in the Data Base");
    }
}
