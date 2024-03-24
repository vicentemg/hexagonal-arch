using RewardEat.Application.Features.CollectedBalanceChallenge.Commands;
using RewardEat.Application.Providers;
using RewardEat.Domain.Primitives;
using RewardEat.Domain.SeedWork;
using Microsoft.Extensions.Logging;
using Moq;
using Aggregates = RewardEat.Domain.Aggregates.CollectedBalanceChallengeAggregate;

namespace RewardEat.Application.Test.Features.CollectedBalanceChallenge.Commands;

public class AddChallengeParticipationCommandHandlerTest
{
    private readonly Mock<IGuidProvider> _guidProviderMock;
    private readonly Mock<Aggregates.ICollectedBalanceChallengeRepository> _repositoryMock;
    private readonly AddChallengeParticipationCommandHandler _sut;

    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public AddChallengeParticipationCommandHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _guidProviderMock = new Mock<IGuidProvider>();

        _repositoryMock = new Mock<Aggregates.ICollectedBalanceChallengeRepository>();
        _repositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(_unitOfWorkMock.Object);

        Mock<ILogger<AddChallengeParticipationCommandHandler>> loggerMock = new();

        _sut = new AddChallengeParticipationCommandHandler(_repositoryMock.Object, loggerMock.Object,
            _guidProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenThereAreMultipleChallengesToBeAddressed_ShouldCallAddParticipationForAllOfThem()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var participationIdOne = Guid.NewGuid();
        var participationIdTwo = Guid.NewGuid();

        var partcipationAmount = 25;

        var challengeOne = CreateCollectedBalanceChallenge("Challenge One", 100);
        var challengeTwo = CreateCollectedBalanceChallenge("Challenge Two", 50);

        _guidProviderMock
            .SetupSequence(s => s.NewId())
            .Returns(participationIdOne)
            .Returns(participationIdTwo);

        _repositoryMock
            .Setup(m => m.GetChallengesByUserId(userId))
            .ReturnsAsync(new List<Aggregates.CollectedBalanceChallenge>
            {
                challengeOne,
                challengeTwo
            });

        var command = new AddChallengeParticipationCommand(userId,
            Guid.NewGuid(),
            partcipationAmount,
            DateTime.Now);
        //Act
        var result = await _sut.Handle(command, default);


        //Assert
        Assert.True(result.IsSuccess);

        var participations = challengeOne.Participations
            .Concat(challengeTwo.Participations);

        Assert.Collection(
            participations,
            participationOne => Assert.Equal(participationOne.UserId, userId),
            p2 => Assert.Equal(p2.UserId, userId)
        );

        Assert.Collection(
            result.Value!.ParticipationIds,
            id => Assert.Equal(id, participationIdOne),
            id => Assert.Equal(id, participationIdTwo)
        );

        _unitOfWorkMock.Verify(m =>
                m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private Aggregates.CollectedBalanceChallenge CreateCollectedBalanceChallenge(string name, decimal amount)
    {
        var id = Guid.NewGuid();
        var challengeName = ChallengeName.Create(name);
        var constraint = Aggregates.CollectedBalanceConstraint.Create(10, amount);

        var challenge = Aggregates.CollectedBalanceChallenge
            .Create(id, challengeName, constraint, DateTime.Now.AddHours(-1)).Value;
        return challenge!;
    }
}
