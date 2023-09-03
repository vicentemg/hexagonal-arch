using MediatR;

namespace HexagonalArch.Application.Events.Integration;

public interface IIntegrationEvent : INotification
{
    Guid CorrelationId { get; }
}
