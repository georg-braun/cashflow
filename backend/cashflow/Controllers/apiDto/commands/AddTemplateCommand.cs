namespace budget_backend.Controllers.apiDto.commands;

public record AddTemplateCommand
(
    double Amount,
    TimeSpan Interval,
    string Note,
    Guid CategoryId
);