using MediatR;

namespace HexagonalArch.Application.Events.Integration.Handlers;

public class IntegrationStartedEventHandler : INotificationHandler<EarnedPoints>
{
    public Task Handle(EarnedPoints @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
