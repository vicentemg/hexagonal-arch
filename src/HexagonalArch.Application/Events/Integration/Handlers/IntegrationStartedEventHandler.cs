using MediatR;

namespace HexagonalArch.Application.Events.Integration.Handlers;

public class IntegrationStartedEventHandler : INotificationHandler<PointsEarned>
{
    public Task Handle(PointsEarned @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
