using HexagonalArch.Domain.SeedWork;
using MediatR;

namespace HexagonalArch.Application.Features.CollectedBalanceChallenge.Commands;

public record class AddAmountParticipationCommand(
    Guid UserId,
    Guid TransactionId,
    decimal Amount,
    DateTime OperationDateTime) : IRequest<Result<AddAmountParticipationCommand.Response>>
{
    public record Response(IEnumerable<Guid> ParticipationsIds);
}