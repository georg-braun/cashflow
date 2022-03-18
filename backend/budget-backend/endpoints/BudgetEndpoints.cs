using budget_backend.application;
using budget_backend.Controllers;
using budget_backend.Controllers.apiDto;
using budget_backend.Controllers.apiDto.commands;
using budget_backend.Controllers.apiDto.datacontainer;
using budget_backend.domain.account;
using budget_backend.domain.budget;
using Microsoft.AspNetCore.Mvc;

namespace budget_backend.endpoints;

public static class BudgetEndpoints
{
    public static async Task<IResult> AddBudgetaryItem(IAccountService accountService, AddNewBudgetaryItemDto newBudgetaryItemDto)
    {
        var budgetaryItem = await accountService.AddBudgetaryItemAsync(newBudgetaryItemDto.Name);
        var budgetDataDto = new BudgetDataApiDto {BudgetaryItem = new[] {budgetaryItem.ToApiDto()}};
        return Results.Created("fillUrl", budgetDataDto);
    }

   
    public static IEnumerable<BudgetaryItemDto> GetAllBudgetaryItems(IAccountService accountService)
    {
        return accountService.GetBudgetaryItems().Select(_ => _.ToApiDto());
    }


    public static IEnumerable<BudgetEntryApiDto> GetBudgetChanges(IAccountService accountService, string budgetaryItemId)
    {
        var typedBudgetaryItemId = BudgetaryItemIdFactory.Create(Guid.Parse(budgetaryItemId));
        return accountService.GetBudgetEntries(typedBudgetaryItemId).Select(BudgetEntryApiDto.ToApiDto);
    }



    public static async Task<IResult> AddBudgetEntry(IAccountService accountService, SetBudgetEntryDto item)
    {
        var budgetaryItemId = BudgetaryItemIdFactory.Create(item.BudgetaryItemid);
        var budgetEntry = await accountService.AddBudgetEntryAsync(budgetaryItemId, item.Amount, item.Month);
        var budgetDataDto = new BudgetDataApiDto {BudgetEntries = new[] {BudgetEntryApiDto.ToApiDto(budgetEntry)}};
        return Results.Created("fillUrl", budgetDataDto);
    }
}