using RewardEat.Application.Events;
using RewardEat.Domain.Aggregates.CollectedBalanceChallengeAggregate;
using RewardEat.Domain.Primitives;

namespace RewardEat.Adapter.Persistence.Test;

public class RewardEatDbContextTest : IClassFixture<RewardEatDbContextFixture>
{
    private readonly Mock<IEventDispatcher> _eventDispatcherMock;
    private readonly RewardEatDbContext _sut;

    public RewardEatDbContextTest(RewardEatDbContextFixture fixture)
    {
        _sut = fixture.DbContext;
        _eventDispatcherMock = fixture.DomainEventDispatcherMock;
    }

    [Fact]
    public async Task SaveChangesAsync_WhenSomeEntitiesWithDomainEventsWereModifiedOrAdded_ShouldDispatchDomainEvents()
    {
        //Arrange
        var constraint = CollectedBalanceConstraint
            .Create(1, 100);

        var challenge = CollectedBalanceChallenge
            .Create(Guid.NewGuid(), ChallengeName.Create("NewChallenge"), constraint, DateTime.Now);

        _sut.CollectedBalanceChallenges.Add(challenge);

        var @event = challenge.Value!.DomainEvents.First();

        //Act
        var numberOfEntities = await _sut.SaveChangesAsync();

        //Assert
        _eventDispatcherMock.Verify(e => e.DispatchAsync(@event, default), Times.Once);

        Assert.Collection(
            _sut.CollectedBalanceConstraints,
            constraintOne => Assert.Equal(constraintOne.ChallengeId, challenge.Value.Id));

        Assert.Equal(2, numberOfEntities);
    }
}
