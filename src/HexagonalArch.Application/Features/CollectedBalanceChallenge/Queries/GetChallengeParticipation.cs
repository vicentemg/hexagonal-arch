namespace HexagonalArch.Application.Features.CollectedBalanceChallenge.Queries;

public record GetChallengeParticipationQuery(Guid Id) : IRequest<GetChallengeParticipationQuery.Response>
{
    public record Response
    {
        public Guid Id { get; init; }
        public Guid TransactionId { get; init; }
        public DateTime OccurredOn { get; init; }
    }
}