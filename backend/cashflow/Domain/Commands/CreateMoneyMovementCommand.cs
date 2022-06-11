using budget_backend.data;
using budget_backend.data.dbo;
using MediatR;

namespace budget_backend.Domain.Commands;

public class CreateMoneyMovementCommand : IRequest<ChangesContainer> {
    public CreateMoneyMovementCommand(MoneyMovement moneyMovement)
    {
        MoneyMovement = moneyMovement;
    }

    public MoneyMovement MoneyMovement { get; init; }
}

public class CreateMoneyMovementCommandHandler : IRequestHandler<CreateMoneyMovementCommand, ChangesContainer>
{
    private readonly CashflowDataContext _cashflowDataContext;

    public CreateMoneyMovementCommandHandler(CashflowDataContext cashflowDataContext)
    {
        _cashflowDataContext = cashflowDataContext;
    }

    public async Task<ChangesContainer> Handle(CreateMoneyMovementCommand request, CancellationToken cancellationToken)
    {
        return await _cashflowDataContext.AddMoneyMovementAsync(request.MoneyMovement);
    }
}