using budget_backend.application;
using budget_backend.data;
using budget_backend.data.dbo;
using MediatR;

namespace budget_backend.Domain.Queries;

public class GetTemplatesQuery : IRequest<IEnumerable<Template>> {
    public GetTemplatesQuery(UserId userId)
    {
        UserId = userId;
    }

    public UserId UserId { get; init; }
}

public class GetTemplatesQueryHandler : IRequestHandler<GetTemplatesQuery, IEnumerable<Template>>
{
    private readonly CashflowDataContext _cashflowDataContext;

    public GetTemplatesQueryHandler(CashflowDataContext cashflowDataContext)
    {
        _cashflowDataContext = cashflowDataContext;
    }

    public async Task<IEnumerable<Template>> Handle(GetTemplatesQuery request, CancellationToken cancellationToken)
    {
        return await _cashflowDataContext.GetTemplates(request.UserId);
    }
}