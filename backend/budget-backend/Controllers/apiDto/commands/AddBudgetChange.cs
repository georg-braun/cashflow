namespace budget_backend.Controllers.apiDto.commands;

public record AddBudgetChangeDto(Guid BudgetaryItemId, double Amount, DateTime Date);