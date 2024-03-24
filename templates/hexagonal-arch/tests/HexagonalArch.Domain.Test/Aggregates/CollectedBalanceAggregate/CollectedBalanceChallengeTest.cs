using HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;
using HexagonalArch.Domain.Events;
using HexagonalArch.Domain.Primitives;

namespace HexagonalArch.Domain.Test.Aggregates.CollectedBalanceAggregate;

public class CollectedBalanceChallengeTest
{
    [Fact]
    public void
        AddParticipation_WhenTheSumOfAllParticipationsReachesTheConstraintAmout_ShouldAddAnAccomplishedChallengeEvent()
    {
        //Arrange
        decimal amount = 100;
        ushort backwardDays = 10;
        var constraint = CollectedBalanceConstraint.Create(backwardDays, amount);

        var challenge = CollectedBalanceChallenge.Create(
            Guid.NewGuid(),
            ChallengeName.Create("ChallengeName"),
            constraint,
            DateTime.Now)!.Value;

        var challengeId = challenge!.Id;
        var userId = Guid.NewGuid();

        var participationOne = CollectedBalanceChallengeParticipation.Create(
            Guid.NewGuid(),
            userId,
            challengeId,
            Guid.NewGuid(),
            10.5m,
            DateTime.Now).Value!;

        var participationTwo = CollectedBalanceChallengeParticipation.Create(
            Guid.NewGuid(),
            userId,
            challengeId,
            Guid.NewGuid(),
            90.5m,
            DateTime.Now).Value!;

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

        Assert.Collection(
            challenge.Participations,
            participationOne => Assert.False(participationOne.IsWinner),
            participationTwo => Assert.True(participationTwo.IsWinner)
        );
    }

    [Fact]
    public void AddParticipation_WhenAChallengeHasBeenAccomplishedDuringItsPeriod_ShouldNotAddTransaction()
    {
        //Arrange
        decimal amount = 100;
        ushort backwardDays = 10;
        var constraint = CollectedBalanceConstraint.Create(backwardDays, amount);

        var challenge = CollectedBalanceChallenge.Create(
            Guid.NewGuid(),
            ChallengeName.Create("ChallengeName"),
            constraint,
            DateTime.Now).Value;

        var userId = Guid.NewGuid();

        var winnerPaticipation = CollectedBalanceChallengeParticipation.Create(
            Guid.NewGuid(),
            userId,
            challenge!.Id,
            Guid.NewGuid(),
            100.5m,
            DateTime.Now).Value!;

        var participationToBeOmitted = CollectedBalanceChallengeParticipation.Create(
            Guid.NewGuid(),
            userId,
            challenge!.Id,
            Guid.NewGuid(),
            90.5m,
            DateTime.Now).Value!;

        var winnerResult = challenge.AddParticipation(winnerPaticipation);
        //Act
        var omittedResult = challenge.AddParticipation(participationToBeOmitted);

        //Assert
        Assert.True(winnerResult.IsSuccess);
        Assert.True(omittedResult.IsSuccess);

        Assert.Collection(
            challenge.Participations,
            winner => Assert.True(winner.IsWinner)
        );
    }

    [Fact]
    public void AddParticipation_WhenTryingToAddAParticipationWithDateBeforeChallengeCreation_ShouldNotAddTransaction()
    {
        //Arrange
        decimal amount = 100;
        ushort backwardDays = 10;
        var constraint = CollectedBalanceConstraint.Create(backwardDays, amount);
        var challengeCreatedOn = DateTime.Now;

        var challenge = CollectedBalanceChallenge.Create(
            Guid.NewGuid(),
            ChallengeName.Create("ChallengeName"),
            constraint,
            challengeCreatedOn).Value!;

        var userId = Guid.NewGuid();

        var participationDate = challengeCreatedOn.Subtract(TimeSpan.FromHours(3));

        var participationToBeOmitted = CollectedBalanceChallengeParticipation.Create(
            Guid.NewGuid(),
            userId,
            challenge.Id,
            Guid.NewGuid(),
            90.5m,
            participationDate).Value!;

        //Act
        var omittedResult = challenge.AddParticipation(participationToBeOmitted);

        //Assert
        Assert.True(omittedResult.IsSuccess);

        Assert.Empty(challenge.Participations);
    }
}
