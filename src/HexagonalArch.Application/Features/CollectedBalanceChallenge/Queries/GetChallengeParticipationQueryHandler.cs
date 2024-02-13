namespace HexagonalArch.Application.Features.CollectedBalanceChallenge.Queries;

using Response = GetChallengeParticipationQuery.Response;

public class GetChallengeParticipationQueryHandler : IRequestHandler<GetChallengeParticipationQuery, Response>
{
    public Task<Response> Handle(GetChallengeParticipationQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
