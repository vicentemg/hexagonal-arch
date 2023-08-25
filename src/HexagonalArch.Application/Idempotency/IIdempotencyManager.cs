namespace HexagonalArch.Application.Idempotency;

public interface IIdempotencyManager
{
    Task<bool> Exist(Guid id);
    Task CreateRequest(Request request, CancellationToken cancellationToken);
}