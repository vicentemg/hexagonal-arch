﻿using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Application.Events;

public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}
