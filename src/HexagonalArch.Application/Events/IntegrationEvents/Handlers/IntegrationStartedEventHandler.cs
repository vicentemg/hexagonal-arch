namespace HexagonalArch.Application.Events.IntegrationEvents.Handlers;

public class IntegrationStartedEventHandler : IIntegrationEventHandler<IntegrationStartedEvent>
{
    public Task HandleAsync(IntegrationStartedEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
