using HexagonalArch.Application.Features;
using HexagonalArch.Application.Features.CollectedBalanceChallenge.Queries;

namespace HexagonalArch.Adapter.Http.Endpoints;

internal static class ChallengeParticipationEndpoints
{
    private static readonly string EndPoint = "challenge-participation";

    internal static WebApplication MapChallengeParticipationEndPoints(this WebApplication app)
    {
        app.MapGet($"{EndPoint}/{{id}}", GetChallengeParticipation);

        return app;
    }

    internal static async Task<IResult> GetChallengeParticipation(
        IRequestHandler<GetChallengeParticipationQuery, GetChallengeParticipationQuery.Response> requestHandler,
        Guid participationId)
    {
        var result = await requestHandler.Handle(new GetChallengeParticipationQuery(Guid.NewGuid()), default);

        return result is not null ?
            Results.Ok(result)
            : Results.NotFound();
    }

    // internal static async Task<IResult> AddParticipation(IMediator mediator, AddChallengeParticipationCommand command)
    // {
    //     var result = await mediator.Send(command);

    //     if (!result.IsSuccess)
    //     {
    //         var detail = result.Errors.First();
    //         return Results.Problem(detail: detail);
    //     }
    //     return Results.Created($"", result.Value);
    // }
}
