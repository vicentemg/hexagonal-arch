using MediatR;

namespace HexagonalArch.Application.Features.Transactions.Commands;
public record class AddTransactionCommand(
    decimal Amount,
    DateTime OperationDateTime) : IRequest<AddTransactionCommand.Result>
{
    public record Result(Guid TransactionId);
}
