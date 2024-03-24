namespace RewardEat.Application.Events.Integration;

public interface IIntegrationEvent
{
    Guid EventId { get; }
}
