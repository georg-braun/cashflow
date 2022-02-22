using System.ComponentModel.DataAnnotations;
using budget_backend.domain;
using budget_backend.domain.account;
using budget_backend.domain.budget;

namespace budget_backend.data.dbDto;

public record BudgetChangeDto(Guid Id, Guid BudgetaryItemId, double Amount, DateOnly Date);

public static class BudgetChangeDtoExtensions
{
    public static BudgetChangeDto ToDbDto(this BudgetChange item) =>
        new(item.Id, item.BudgetaryItemId, item.Amount, item.Date);


    public static BudgetChange ToDomain(this BudgetChangeDto item) =>
        new (item.Id, item.BudgetaryItemId, item.Date, item.Amount);
}