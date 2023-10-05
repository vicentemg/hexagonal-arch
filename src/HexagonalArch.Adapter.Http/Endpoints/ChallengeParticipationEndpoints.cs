using HexagonalArch.Application.Features.CollectedBalanceChallenge.Commands;
using HexagonalArch.Application.Features.CollectedBalanceChallenge.Queries;

namespace HexagonalArch.Adapter.Http.Endpoints;

internal static class ChallengeParticipationEndpoints
{
    private const string EndPoint = "api/challenge-participation";

    internal static WebApplication MapChallgenParticipation(this WebApplication app)
    {
        // app.MapGet($"{EndPoint}/{{id}}", GetChallengeParticipation);
        // app.MapPost(EndPoint, AddParticipation);

        return app;
    }

    // internal static async Task<IResult> GetChallengeParticipation(IMediator mediator, Guid participationId)
    // {
    //     var result = await mediator.Send(new GetChallengeParticipationQuery(Guid.NewGuid()));

    //     return result is not null ?
    //         Results.Ok(result)
    //         : Results.NotFound();
    // }

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