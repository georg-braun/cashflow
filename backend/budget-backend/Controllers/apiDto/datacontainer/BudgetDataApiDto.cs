using System.Collections.ObjectModel;

namespace budget_backend.Controllers.apiDto.datacontainer;

public record BudgetDataApiDto
{
    public ICollection<AccountApiDto> Accounts { get; init; } = new List<AccountApiDto>();
    public ICollection<Guid> DeletedAccountIds { get; init; } = new List<Guid>();
    public ICollection<AccountEntryApiDto> AccountEntries { get; init; } = new List<AccountEntryApiDto>();
    
    public ICollection<Guid> DeletedAccountEntryIds { get; init; } = new List<Guid>();
    public ICollection<BudgetaryItemDto> BudgetaryItems { get; init; } = new List<BudgetaryItemDto>();
    public ICollection<Guid> DeletedBudgetaryItemIds { get; init; } = new List<Guid>();
    public ICollection<BudgetEntryApiDto> BudgetEntries { get; init; } = new List<BudgetEntryApiDto>();
    
    
    public ICollection<Guid> DeletedBudgetEntryIds { get; init; } = new List<Guid>();
    public ICollection<SpendingDto> Spendings { get; init; } = new List<SpendingDto>();

    
}