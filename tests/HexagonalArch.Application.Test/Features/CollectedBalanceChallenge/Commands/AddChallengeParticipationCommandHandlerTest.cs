using HexagonalArch.Application.Features.CollectedBalanceChallenge.Commands;
using HexagonalArch.Application.Providers;
using HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;
using HexagonalArch.Domain.Primitives;
using HexagonalArch.Domain.SeedWork;
using Microsoft.Extensions.Logging;
using Moq;
using Aggregates = HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;

namespace HexagonalArch.Application.Test.Features.CollectedBalanceChallenge.Commands;

public class AddChallengeParticipationCommandHandlerTest
{
    private readonly AddChallengeParticipationCommandHandler _sut;

    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGuidProvider> _guidProviderMock;
    private readonly Mock<ICollectedBalanceChallengeRepository> _repositoryMock;
    private readonly Mock<ILogger<AddChallengeParticipationCommandHandler>> _loggerMock;

    public AddChallengeParticipationCommandHandlerTest()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _guidProviderMock = new Mock<IGuidProvider>();

        _repositoryMock = new Mock<ICollectedBalanceChallengeRepository>();
        _repositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(_unitOfWorkMock.Object);

        _loggerMock = new Mock<ILogger<AddChallengeParticipationCommandHandler>>();

        _sut = new AddChallengeParticipationCommandHandler(_repositoryMock.Object, _loggerMock.Object, _guidProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenThereAreMultpleChallengesToBeAddressed_ShouldCallAddParticipationForAllOfThem()
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
            .ReturnsAsync(new List<Aggregates.CollectedBalanceChallenge>{
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
            p1 => Assert.Equal(p1.UserId, userId),
            p2 => Assert.Equal(p2.UserId, userId)
        );

        Assert.Collection(
            result.Value.ParticipationsIds,
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
        var constraint = new CollectedBalanceConstraint(1, 10, amount);

        return Aggregates.CollectedBalanceChallenge
            .Create(id, challengeName, constraint, DateTime.Now.AddHours(-1));
    }
}
