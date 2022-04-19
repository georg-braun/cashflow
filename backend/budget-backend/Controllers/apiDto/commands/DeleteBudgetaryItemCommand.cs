namespace budget_backend.Controllers.apiDto.commands;

public record DeleteBudgetaryItemCommand
{
    public Guid BudgetaryItemId { get; set; }
}