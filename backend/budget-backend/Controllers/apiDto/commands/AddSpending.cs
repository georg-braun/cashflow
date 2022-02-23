namespace budget_backend.Controllers.apiDto.commands;

public record AddSpending(Guid AccountId, Guid BudgetaryItemId, double Amount, DateTime Date);
