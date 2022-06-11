using budget_backend.application;
using budget_backend.data;
using budget_backend.data.dbo;
using MediatR;

namespace budget_backend.Domain.Commands;

public class DeleteCategoryCommand : IRequest<ChangesContainer> {
    public DeleteCategoryCommand(CategoryId id, UserId userId)
    {
        Id = id;
        UserId = userId;
    }

    public CategoryId Id { get; init; }
    
    public UserId UserId { get; init; }
}

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ChangesContainer>
{
    private readonly CashflowDataContext _cashflowDataContext;

    public DeleteCategoryCommandHandler(CashflowDataContext cashflowDataContext)
    {
        _cashflowDataContext = cashflowDataContext;
    }

    public async Task<ChangesContainer> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var deleteCategoryChanges = await _cashflowDataContext.DeleteCategoryAsync(request.Id, request.UserId);
        var deleteMoneyMovementsChanges = await _cashflowDataContext.DeleteMoneyMovementsForCategory(request.Id);
        await _cashflowDataContext.SaveChangesAsync(cancellationToken);
        
        return deleteCategoryChanges.Union(deleteMoneyMovementsChanges);
    }
}