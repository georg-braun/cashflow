using System.ComponentModel.DataAnnotations;
using budget_backend.application;
using budget_backend.domain;
using budget_backend.domain.account;
using budget_backend.domain.budget;

namespace budget_backend.data.dbDto;

public class BudgetEntryDto
{
    [Key]
    public Guid Id { get; init; }
    public Guid BudgetaryItemId { get; init; }
    public double Amount { get; init; }
    public DateTime Month { get; init; }
    public Guid UserId { get; init; }
}

public static class BudgetChangeDtoExtensions
{
    public static BudgetEntryDto ToDbDto(this BudgetEntry item, UserId userId) =>
        new()
        {
            Id = item.Id.Id, 
            BudgetaryItemId = item.BudgetaryItemId.Id, 
            Amount = item.Amount,
            Month = item.Month,
            UserId = userId.Id
        };


    public static BudgetEntry ToDomain(this BudgetEntryDto item) =>
        new (BudgetEntryIdFactory.Create(item.Id), BudgetaryItemIdFactory.Create(item.BudgetaryItemId), item.Month, item.Amount);
}