using System.Security.Claims;
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
    public static async Task<IResult> AddBudgetaryItem(IAccountService accountService, IUserService userService, ClaimsPrincipal claims, AddNewBudgetaryItemDto newBudgetaryItemDto)
    {
        var userId = await userService.GetUserIdAsync(EndpointUtilities.ExtractAuthUserId(claims));
        var budgetaryItem = await accountService.AddBudgetaryItemAsync(newBudgetaryItemDto.Name, userId);
        var budgetDataDto = new BudgetDataApiDto {BudgetaryItem = new[] {budgetaryItem.ToApiDto()}};
        return Results.Created("fillUrl", budgetDataDto);
    }

   
    public static async Task<IEnumerable<BudgetaryItemDto>> GetAllBudgetaryItems(IAccountService accountService, IUserService userService, ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(EndpointUtilities.ExtractAuthUserId(claims));
        return accountService.GetBudgetaryItems(userId).Select(_ => _.ToApiDto());
    }


    public static async Task<IEnumerable<BudgetEntryApiDto>> GetBudgetChanges(IAccountService accountService, IUserService userService, ClaimsPrincipal claims, string budgetaryItemId)
    {
        var userId = await userService.GetUserIdAsync(EndpointUtilities.ExtractAuthUserId(claims));
        var typedBudgetaryItemId = BudgetaryItemIdFactory.Create(Guid.Parse(budgetaryItemId));
        return accountService.GetBudgetEntries(typedBudgetaryItemId, userId).Select(BudgetEntryApiDto.ToApiDto);
    }



    public static async Task<IResult> AddBudgetEntry(IAccountService accountService, IUserService userService, ClaimsPrincipal claims, SetBudgetEntryDto item)
    {
        var userId = await userService.GetUserIdAsync(EndpointUtilities.ExtractAuthUserId(claims));
        var budgetaryItemId = BudgetaryItemIdFactory.Create(item.BudgetaryItemid);
        var budgetEntry = await accountService.AddBudgetEntryAsync(budgetaryItemId, item.Amount, item.Month, userId);
        var budgetDataDto = new BudgetDataApiDto {BudgetEntries = new[] {BudgetEntryApiDto.ToApiDto(budgetEntry)}};
        return Results.Created("fillUrl", budgetDataDto);
    }
}