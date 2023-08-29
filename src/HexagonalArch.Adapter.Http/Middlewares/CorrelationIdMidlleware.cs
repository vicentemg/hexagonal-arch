using HexagonalArch.Application.Idempotency;
using HexagonalArch.Application.Providers;

namespace HexagonalArch.Adapter.Http.Middlewares;

public class CorrelationIdMidlleware : IMiddleware
{
    internal const string HeaderCorrelationId = "x-correlation-id";
    private readonly IIdempotencyService _idempotencyManager;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CorrelationIdMidlleware(
        IIdempotencyService idempotencyManager,
        IDateTimeProvider dateTimeProvider)
    {
        _idempotencyManager = idempotencyManager;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var request = context.Request;
        _ = request.Headers.TryGetValue(HeaderCorrelationId, out var correlationId);

        if (string.IsNullOrEmpty(correlationId)
            || !Guid.TryParse(correlationId, out Guid guid)
            || await _idempotencyManager.HasBeenProcessed(guid))
        {
            throw new InvalidOperationException("Invalid correlation id header");
        }

        await _idempotencyManager.AddRequestAsync(new Request(guid, request.Path, _dateTimeProvider.UtcNow), context.RequestAborted);

        await next(context);
    }
}