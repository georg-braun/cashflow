using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using budget_backend.application;
using budget_backend.domain;
using budget_backend.domain.account;
using budget_backend.domain.budget;

namespace budget_backend.data.dbDto;

public class SpendingDto
{
    
    public Guid AccountEntryId { get; init; }
    public Guid BudgetaryItemId { get; init; }
    public Guid AccountId { get; init; }
    public Guid UserId { get; set; }
}

public static class SpendingDtoExtensions
{
    public static SpendingDto ToDbDto(this Spending item, UserId userId) =>
        new()
        {
            AccountId = item.AccountId.Id, 
            AccountEntryId = item.AccountEntryId.Id,
            BudgetaryItemId = item.BudgetaryItemId.Id,
            UserId = userId.Id
        };


    public static Spending ToDomain(this SpendingDto item) =>
        new (AccountIdFactory.Create(item.AccountId), new AccountEntryId(){Id = item.AccountEntryId}, BudgetaryItemIdFactory.Create(item.BudgetaryItemId));
}