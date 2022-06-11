using budget_backend.data;
using budget_backend.data.dbo;
using MediatR;

namespace budget_backend.Domain.Commands;

public class CreateCategoryCommand : IRequest<ChangesContainer> {
    public CreateCategoryCommand(Category category)
    {
        Category = category;
    }

    public Category Category { get; init; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ChangesContainer>
{
    private readonly CashflowDataContext _cashflowDataContext;

    public CreateCategoryCommandHandler(CashflowDataContext cashflowDataContext)
    {
        _cashflowDataContext = cashflowDataContext;
    }

    public async Task<ChangesContainer> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var changes = await _cashflowDataContext.AddCategoryAsync(request.Category);
        await _cashflowDataContext.SaveChangesAsync(cancellationToken);
        return changes;
    }
}