using HexagonalArch.Application.Features;
using HexagonalArch.Application.Features.CollectedBalanceChallenge.Queries;
using HexagonalArch.Domain.SeedWork;

namespace HexagonalArch.Adapter.Http.Endpoints;

internal static class ChallengeParticipationEndpoints
{
    private const string EndPoint = "challenge-participation";

    internal static WebApplication MapChallengeParticipationEndPoints(this WebApplication app)
    {
        app.MapGet($"{EndPoint}/{{id}}", GetChallengeParticipation);

        return app;
    }

    internal static async Task<IResult> GetChallengeParticipation(
        IRequestHandler<GetChallengeParticipationQuery, Result<GetChallengeParticipationQuery.Response>> requestHandler,
        Guid participationId)
    {
        var result = await requestHandler.Handle(new GetChallengeParticipationQuery(Guid.NewGuid()), default);

        return result is not null ?
            Results.Ok(result)
            : Results.NotFound();
    }
}
