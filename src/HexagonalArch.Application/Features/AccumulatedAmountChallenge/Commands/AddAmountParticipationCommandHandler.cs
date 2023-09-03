using HexagonalArch.Domain.Aggregates.CollectedBalanceChallengeAggregate;
using MediatR;

namespace HexagonalArch.Application.Features.AccumulatedAmountChallenge.Commands;

public class AddAmountParticipationCommandHandler : IRequestHandler<AddAmountParticipationCommand, AddAmountParticipationCommand.Result>
{
    private readonly ICollectedBalanceChallengeRepository _transactionRepository;

    public AddAmountParticipationCommandHandler(ICollectedBalanceChallengeRepository repository)
    {
        _transactionRepository = repository;
    }

    public async Task<AddAmountParticipationCommand.Result> Handle(AddAmountParticipationCommand request, CancellationToken cancellationToken)
    {
        var unitOfWork = _transactionRepository.UnitOfWork;

        var result = new AddAmountParticipationCommand.Result(Guid.NewGuid());


        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}
