using MediatR;

namespace HexagonalArch.Application.Features.AccumulatedAmountChallenge.Commands;
public record class AddAmountParticipationCommand(
    decimal Amount,
    DateTime OperationDateTime) : IRequest<AddAmountParticipationCommand.Result>
{
    public record Result(Guid TransactionId);
}
