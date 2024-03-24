using RewardEat.Application.Events.Domain;
using RewardEat.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace RewardEat.Application.Test;

public class ApplicationModuleTest
{
    [Fact]
    public void AddDomainEventHandlers_WhenThereIsHandlersInAnAssemble_ShouldAddThemToServicesCollection()
    {
        //Arrange
        var assembly = typeof(HandlerOne).Assembly;
        var services = new ServiceCollection();

        //Act
        services.AddEventHandlers(assembly);

        //Assert
        Assert.NotEmpty(services);
        Assert.Collection(
            services,
            serviceDescriptor =>
            {
                Assert.Equal(typeof(IDomainEventHandler<CollectedBalanceChallengeCreated>),
                    serviceDescriptor.ServiceType);
                Assert.Equal(typeof(HandlerOne), serviceDescriptor.ImplementationType);
            });
    }
}

internal class HandlerOne : IDomainEventHandler<CollectedBalanceChallengeCreated>
{
    public Task Handle(CollectedBalanceChallengeCreated @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
