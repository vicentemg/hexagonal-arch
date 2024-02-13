using HexagonalArch.Adapter.Http.Contexts;
using HexagonalArch.Application.Providers;
using HexagonalArch.Application.Services;

namespace HexagonalArch.Adapter.Http.Endpoints.Filters;

public class IdempotentRequestFilter : IEndpointFilter
{
    internal const string IdempotencyKeyHeader = "Idempotency-Key";

    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IHttpHeaderContext _httpHeaderContext;
    private readonly IIdempotencyService _idempotencyService;

    public IdempotentRequestFilter(
        IIdempotencyService idempotencyService,
        IDateTimeProvider dateTimeProvider,
        IHttpHeaderContext httpHeaderContext)
    {
        _dateTimeProvider = dateTimeProvider;
        _httpHeaderContext = httpHeaderContext;
        _idempotencyService = idempotencyService;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var headerCorrelationId = _httpHeaderContext.GetValue(IdempotencyKeyHeader);

        if (string.IsNullOrEmpty(headerCorrelationId)
            || !Guid.TryParse(headerCorrelationId, out var correlationId))
            return Results.Problem(
                $"Invalid value for header {headerCorrelationId}",
                statusCode: StatusCodes.Status400BadRequest
            );

        if (await _idempotencyService.HasBeenProcessed(correlationId))
            return Results.Problem(
                $"The request id:{correlationId} has been already processed",
                statusCode: StatusCodes.Status403Forbidden
            );

        var pathRequest = context.HttpContext.Request.Path;
        var cancellationToken = context.HttpContext.RequestAborted;
        var idempotentRequest = new IdempotentRequest(correlationId, pathRequest, _dateTimeProvider.UtcNow);

        await _idempotencyService.AddRequestAsync(idempotentRequest, cancellationToken);

        return await next(context);
    }
}
