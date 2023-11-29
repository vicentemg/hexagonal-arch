using HexagonalArch.Application.Events;
using HexagonalArch.Domain.Events;
using HexagonalArch.Domain.SeedWork;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HexagonalArch.Application.Test.Events.Domain;

public class DomainEventDispatcherTest
{
    [Fact]
    public async Task Dispatch_WhenEventParamIsNotIDomainEventType_ShouldThrowAnException()
    {
        //Arrange
        var notEventTypeObj = new { };
        var messageExpected = $"{notEventTypeObj.GetType()} is not type of {typeof(IDomainEvent)}";
        var sp = new ServiceCollection().BuildServiceProvider();
        var eventDispatcher = new DomainEventDispatcher(sp);

        //Act
        var testCode = async () => await eventDispatcher.DispatchAsync(notEventTypeObj, default);

        //Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(testCode);
        Assert.Equal(messageExpected, exception.Message);
    }

    [Fact]
    public async Task Dispatch_WhenAnEventTypeIsPassed_ShouldCallAllHandlers()
    {
        //Arrange
        var handlerMock = new Mock<IDomainEventHandler<CollectedBalanceChallengeCreated>>();
        var domainEvent = new CollectedBalanceChallengeCreated(Guid.NewGuid());

        var serviceProvider = new ServiceCollection()
            .AddTransient(
                typeof(IDomainEventHandler<CollectedBalanceChallengeCreated>),
                sp => handlerMock.Object)
            .BuildServiceProvider();

        var eventDispatcher = new DomainEventDispatcher(serviceProvider);

        //Act
        await eventDispatcher.DispatchAsync(domainEvent, default);

        //Assert
        handlerMock.Verify(m => m.Handle(domainEvent, default), Times.Once);
    }

    [Fact]
    public async Task Dispatch_WhenThereIsNoHandlerRegisteredToCertainEvents_ShouldNotCallToAnyOne()
    {
        //Arrange
        var domainEvent = new CollectedBalanceChallengeCreated(Guid.NewGuid());
        var handlerMock = new Mock<IDomainEventHandler<AccomplishedCollectedBalanceChallenge>>();

        var serviceProvider = new ServiceCollection()
            .AddTransient(
                typeof(IDomainEventHandler<AccomplishedCollectedBalanceChallenge>),
                sp => handlerMock.Object)
            .BuildServiceProvider();

        var eventDispatcher = new DomainEventDispatcher(serviceProvider);

        //Act
        await eventDispatcher.DispatchAsync(domainEvent, default);

        //Assert
        handlerMock.Verify(
            m => m.Handle(It.IsAny<AccomplishedCollectedBalanceChallenge>(), default),
            Times.Never);
    }
}
