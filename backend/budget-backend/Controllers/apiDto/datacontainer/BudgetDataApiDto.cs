namespace budget_backend.Controllers.apiDto.datacontainer;

public record BudgetDataApiDto
{
    public IEnumerable<AccountApiDto> Accounts { get; init; }
    public IEnumerable<AccountEntryApiDto> AccountEntries { get; init; }
    public IEnumerable<BudgetaryItemDto> BudgetaryItem { get; init; }
    public IEnumerable<BudgetEntryApiDto> BudgetEntries { get; init; }
    public IEnumerable<SpendingDto> Spendings { get; init; }
}