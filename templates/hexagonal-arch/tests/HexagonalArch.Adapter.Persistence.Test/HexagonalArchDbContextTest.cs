using HexagonalArch.Application.Events;
using HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;
using HexagonalArch.Domain.Primitives;

namespace HexagonalArch.Adapter.Persistence.Test;

public class HexagonalArchDbContextTest : IClassFixture<HexagonalArchDbContextFixture>
{
    private readonly Mock<IEventDispatcher> _eventDispatcherMock;
    private readonly HexagonalArchDbContext _sut;

    public HexagonalArchDbContextTest(HexagonalArchDbContextFixture fixture)
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
