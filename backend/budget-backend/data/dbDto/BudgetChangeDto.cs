using System.ComponentModel.DataAnnotations;
using budget_backend.domain;
using budget_backend.domain.account;
using budget_backend.domain.budget;

namespace budget_backend.data.dbDto;

public class BudgetChangeDto
{
    public BudgetChangeDto(Guid id, Guid budgetaryItemId, double amount, DateOnly date)
    {
        Id = id;
        BudgetaryItemId = budgetaryItemId;
        Amount = amount;
        Date = date;
    }

    [Key]
    public Guid Id { get; init; }
    public Guid BudgetaryItemId { get; init; }
    public double Amount { get; init; }
    public DateOnly Date { get; init; }
}

public static class BudgetChangeDtoExtensions
{
    public static BudgetChangeDto ToDbDto(this BudgetChange item) =>
        new(item.Id, item.BudgetaryItemId, item.Amount, item.Date);


    public static BudgetChange ToDomain(this BudgetChangeDto item) =>
        new (item.Id, item.BudgetaryItemId, item.Date, item.Amount);
}