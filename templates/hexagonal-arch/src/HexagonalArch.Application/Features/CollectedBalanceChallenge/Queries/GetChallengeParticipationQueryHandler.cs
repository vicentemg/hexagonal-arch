namespace HexagonalArch.Application.Features.CollectedBalanceChallenge.Queries;

using HexagonalArch.Domain.SeedWork;

public class GetChallengeParticipationQueryHandler : IRequestHandler<GetChallengeParticipationQuery, Result<GetChallengeParticipationQuery.Response>>
{
    public Task<Result<GetChallengeParticipationQuery.Response>> Handle(GetChallengeParticipationQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<Result<GetChallengeParticipationQuery.Response>>(new Error(0, ""));
    }
}
