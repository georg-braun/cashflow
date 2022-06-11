using budget_backend.application;
using budget_backend.data;
using budget_backend.data.dbo;
using MediatR;

namespace budget_backend.Domain.Commands;

public class DeleteMoneyMovementCommand : IRequest<ChangesContainer> {
    public DeleteMoneyMovementCommand(MoneyMovementId id, UserId userId)
    {
        Id = id;
        UserId = userId;
    }

    public MoneyMovementId Id { get; init; }
    
    public UserId UserId { get; init; }
}

public class DeleteMoneyMovementCommandHandler : IRequestHandler<DeleteMoneyMovementCommand, ChangesContainer>
{
    private readonly CashflowDataContext _cashflowDataContext;

    public DeleteMoneyMovementCommandHandler(CashflowDataContext cashflowDataContext)
    {
        _cashflowDataContext = cashflowDataContext;
    }

    public async Task<ChangesContainer> Handle(DeleteMoneyMovementCommand request, CancellationToken cancellationToken)
    {
        var changes = await _cashflowDataContext.DeleteMoneyMovementAsync(request.Id, request.UserId);
        
        
        await _cashflowDataContext.SaveChangesAsync(cancellationToken);
        return changes;

    }
}