namespace HexagonalArch.Application.Idempotency;

public interface IIdempotencyService
{
    Task<bool> HasBeenProcessed(Guid id);
    Task AddRequestAsync(Request request, CancellationToken cancellationToken);
}