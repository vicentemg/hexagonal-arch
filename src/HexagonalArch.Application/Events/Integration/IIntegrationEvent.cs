
namespace HexagonalArch.Application.Events.Integration;

public interface IIntegrationEvent
{
    Guid CorrelationId { get; }
}
