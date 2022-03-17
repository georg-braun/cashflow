namespace budget_backend.Controllers.apiDto.commands;

public record SetBudgetEntryDto(Guid BudgetaryItemid, DateTime Month, double Amount);
