using HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;
using HexagonalArch.Domain.Events;

namespace HexagonalArch.Domain.Test.Aggregates.CollectedBalanceAggregate;

public class CollectedBalanceChallengeTest
{
    [Fact]
    public void AddParticipation_WhenParticipationsAmountsAccumulatedReachConstraintAmout_ShouldAddAnAccomplishedChallengeEvent()
    {
        //Arrange
        var amount = 100m;
        var backwardDays = 10;
        var constraint = new CollectedBalanceConstraint(backwardDays, amount);

        var challenge = CollectedBalanceChallenge.Create(Guid.NewGuid(), constraint, DateTime.Now).Value;

        var userId = Guid.NewGuid();

        var participationOne = CollectedBalanceChallengeParticipation.Create(
            Guid.NewGuid(),
            userId,
            challenge.Id,
            Guid.NewGuid(),
            10.5m,
            DateTime.Now).Value;

        var participationTwo = CollectedBalanceChallengeParticipation.Create(
            Guid.NewGuid(),
            userId,
            challenge.Id,
            Guid.NewGuid(),
            90.5m,
            DateTime.Now).Value;

        //Act
        var participationOneResult = challenge.AddParticipation(participationOne);
        var participationTwoResult = challenge.AddParticipation(participationTwo);

        //Assert
        Assert.True(participationOneResult.IsSuccess);
        Assert.True(participationTwoResult.IsSuccess);
        Assert.Collection(
            challenge.DomainEvents,
            _ => { },
            @event => Assert.IsType<AccomplishedCollectedBalanceChallenge>(@event)
        );

    }
}

