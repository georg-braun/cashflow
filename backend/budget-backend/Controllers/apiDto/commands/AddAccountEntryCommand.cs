namespace budget_backend.Controllers.apiDto.commands;

public record AddAccountEntryCommand
{
    public Guid AccountId { get; init; }
    
    public Guid BudgetaryItemId { get; init; }
    public double Amount { get; init; }
    public DateTime Date { get; init; }

    public string Note { get; init; } = string.Empty;
}