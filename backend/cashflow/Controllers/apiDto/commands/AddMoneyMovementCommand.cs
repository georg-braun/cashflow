namespace budget_backend.Controllers.apiDto.commands;

public record AddMoneyMovementCommand
(
    double Amount,
    DateTime Date,
    string Note,
    Guid CategoryId
);