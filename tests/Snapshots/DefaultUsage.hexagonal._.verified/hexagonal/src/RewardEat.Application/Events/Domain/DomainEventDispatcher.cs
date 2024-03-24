using RewardEat.Domain.SeedWork;
using Microsoft.Extensions.DependencyInjection;

namespace RewardEat.Application.Events.Domain;

public class DomainEventDispatcher : IEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(object @event, CancellationToken cancellationToken)
    {
        var eventType = @event.GetType();

        if (!eventType.IsAssignableTo(typeof(IDomainEvent)))
            throw new ArgumentException($"{eventType} is not type of {typeof(IDomainEvent)}");

        var handlers = GetHandlers(eventType);

        foreach (var handler in handlers)
        {
            var handleTask = Handle(@event, handler, cancellationToken);

            await handleTask.ConfigureAwait(false);
        }
    }

    private IEnumerable<object> GetHandlers(Type eventType)
    {
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

        return _serviceProvider
            .GetServices(handlerType)
            .Where(handler => handler is not null)!;
    }

    private static Task Handle(object @event, object handler, CancellationToken cancellationToken)
    {
        var handleMethod = handler.GetType().GetMethod("Handle");

        ArgumentNullException.ThrowIfNull(handleMethod);

        var task = handleMethod.Invoke(handler, new[] { @event, cancellationToken }) as Task;

        ArgumentNullException.ThrowIfNull(task);

        return task;
    }
}
