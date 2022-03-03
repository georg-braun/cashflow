using System.ComponentModel.DataAnnotations;
using budget_backend.domain;
using budget_backend.domain.account;
using budget_backend.domain.budget;

namespace budget_backend.data.dbDto;

public class BudgetEntryDto
{
    public BudgetEntryDto(Guid id, Guid budgetaryItemId, double amount, DateTime month)
    {
        Id = id;
        BudgetaryItemId = budgetaryItemId;
        Amount = amount;
        Month = month;
    }

    [Key]
    public Guid Id { get; init; }
    public Guid BudgetaryItemId { get; init; }
    public double Amount { get; init; }
    public DateTime Month { get; init; }
}

public static class BudgetChangeDtoExtensions
{
    public static BudgetEntryDto ToDbDto(this BudgetEntry item) =>
        new(item.Id.Id, item.BudgetaryItemId.Id, item.Amount, item.Month);


    public static BudgetEntry ToDomain(this BudgetEntryDto item) =>
        new (BudgetEntryIdFactory.Create(item.Id), BudgetaryItemIdFactory.Create(item.BudgetaryItemId), item.Month, item.Amount);
}