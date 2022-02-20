namespace budget_backend.domain.budget;

public class Spending
{
    public Guid AccountEntryId { get; init; }
    
    public Guid BudgetaryItemId { get; init; }
    public Guid AccountId { get; set; }
}