using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using budget_backend.domain;
using budget_backend.domain.account;
using budget_backend.domain.budget;

namespace budget_backend.data.dbDto;

public class SpendingDto
{
    public SpendingDto(Guid accountId, Guid accountEntryId, Guid budgetaryItemId)
    {
        AccountId = accountId;
        AccountEntryId = accountEntryId;
        BudgetaryItemId = budgetaryItemId;
    }

    
    public Guid AccountEntryId { get; init; }
    public Guid BudgetaryItemId { get; init; }
    public Guid AccountId { get; init; }
}

public static class SpendingDtoExtensions
{
    public static SpendingDto ToDbDto(this Spending? item) =>
        new(item.AccountId, item.AccountEntryId.Id, item.BudgetaryItemId);


    public static Spending ToDomain(this SpendingDto item) =>
        new (item.AccountId, new AccountEntryId(){Id = item.AccountEntryId}, item.BudgetaryItemId);
}