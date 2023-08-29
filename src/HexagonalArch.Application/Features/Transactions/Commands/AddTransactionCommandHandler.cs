using HexagonalArch.Domain.Aggregates.TransactionAggregate;
using MediatR;

namespace HexagonalArch.Application.Features.Transactions.Commands;

public class AddTransactionCommandHandler : IRequestHandler<AddTransactionCommand, AddTransactionCommand.Result>
{
    private readonly IAccumulatedAmountChallengeRepository _transactionRepository;

    public AddTransactionCommandHandler(IAccumulatedAmountChallengeRepository repository)
    {
        _transactionRepository = repository;
    }

    public async Task<AddTransactionCommand.Result> Handle(AddTransactionCommand request, CancellationToken cancellationToken)
    {
        var unitOfWork = _transactionRepository.UnitOfWork;

        var result = new AddTransactionCommand.Result(Guid.NewGuid());


        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}
