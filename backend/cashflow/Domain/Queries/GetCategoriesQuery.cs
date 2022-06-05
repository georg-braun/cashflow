using budget_backend.application;
using budget_backend.data;
using budget_backend.data.dbo;
using MediatR;

namespace budget_backend.Domain.Queries;

public class GetCategoriesQuery : IRequest<IEnumerable<Category>> {
    public GetCategoriesQuery(UserId userId)
    {
        UserId = userId;
    }

    public UserId UserId { get; init; }
}

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<Category>>
{
    private readonly CashflowDataContext _cashflowDataContext;

    public GetCategoriesQueryHandler(CashflowDataContext cashflowDataContext)
    {
        _cashflowDataContext = cashflowDataContext;
    }

    public async Task<IEnumerable<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _cashflowDataContext.GetCategories(request.UserId);
    }
}