namespace budget_backend.Controllers.apiDto.commands;

public record AddIncomeDto
{
    public Guid AccountId { get; init; }
    public double Amount { get; init; }
    public DateTime Date { get; init; }
}