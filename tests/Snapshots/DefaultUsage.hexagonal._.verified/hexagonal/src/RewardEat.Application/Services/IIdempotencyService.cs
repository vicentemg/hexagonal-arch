namespace RewardEat.Application.Services;

public interface IIdempotencyService
{
    Task<bool> HasBeenProcessed(Guid idempotencyKey);
    Task AddRequestAsync(IdempotentRequest request, CancellationToken cancellationToken);
}
