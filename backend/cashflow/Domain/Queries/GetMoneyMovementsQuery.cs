using budget_backend.application;
using budget_backend.data;
using budget_backend.data.dbo;
using MediatR;

namespace budget_backend.Domain.Queries;

public class GetMoneyMovementsQuery : IRequest<IEnumerable<MoneyMovement>> {
    public GetMoneyMovementsQuery(UserId userId)
    {
        UserId = userId;
    }

    public UserId UserId { get; init; }
}

public class GetMoneyMovementsQueryHandler : IRequestHandler<GetMoneyMovementsQuery, IEnumerable<MoneyMovement>>
{
    private readonly CashflowDataContext _cashflowDataContext;

    public GetMoneyMovementsQueryHandler(CashflowDataContext cashflowDataContext)
    {
        _cashflowDataContext = cashflowDataContext;
    }

    public async Task<IEnumerable<MoneyMovement>> Handle(GetMoneyMovementsQuery request, CancellationToken cancellationToken)
    {
        return await _cashflowDataContext.GetMoneyMovements(request.UserId);
    }
}