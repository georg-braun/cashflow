using System.Security.Claims;
using budget_backend.application;
using budget_backend.Controllers.apiDto;
using budget_backend.Controllers.apiDto.commands;
using budget_backend.Controllers.apiDto.datacontainer;
using budget_backend.domain.account;
using budget_backend.domain.budget;
using Microsoft.AspNetCore.Mvc;

namespace budget_backend.endpoints;

public static class AccountEndpoints
{
    public static IEnumerable<AccountApiDto> GetAllAccounts(IAccountService accountService) 
    {
        var accounts = accountService.GetAccounts();
        return accounts.Select(_ => _.ToApiDto());
    }
    

    public static IEnumerable<AccountEntryApiDto> GetAccountEntriesOfAccount(IAccountService accountService, string accountId)
    {
        var accountGuid = AccountIdFactory.Create(Guid.Parse(accountId));
        var accountEntries = accountService.GetAccountEntries(accountGuid);
        return accountEntries.Select(_ => _.ToApiDto());
    }

    public static IEnumerable<SpendingDto> GetSpendings(IAccountService accountService)
    {
        var spendings = accountService.GetSpendings();
        return spendings.Select(_ => _.ToApiDto());
    }

    public static async Task<IResult> AddIncome(IAccountService accountService, AddIncomeDto addIncomeDto)
    {
        var date = new DateOnly(addIncomeDto.Date.Year, addIncomeDto.Date.Month, addIncomeDto.Date.Day);
        var accountEntry = await accountService.AddIncomeAsync(AccountIdFactory.Create(addIncomeDto.AccountId), addIncomeDto.Amount, date);
        if (accountEntry != null)
        {
            var budgetDataDto = new BudgetDataApiDto {AccountEntries = new[] {accountEntry.ToApiDto()}};
            return Results.Created("fillUrl", budgetDataDto);
        }

        return Results.UnprocessableEntity();
    }
    
    public static async Task<IResult> AddSpending(IAccountService accountService, IUserService userService, AddSpending addSpendingDto, ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(ExtractAuthUserId(claims));
        var date = new DateOnly(addSpendingDto.Date.Year, addSpendingDto.Date.Month, addSpendingDto.Date.Day);
        var accountId = AccountIdFactory.Create(addSpendingDto.AccountId);
        var budgetaryItemId = BudgetaryItemIdFactory.Create(addSpendingDto.BudgetaryItemId);
        var spending = await accountService.AddSpendingAsync(accountId, budgetaryItemId, addSpendingDto.Amount, date);

        if (spending is null)
            return Results.UnprocessableEntity();
        
        var accountEntry = await accountService.GetAccountEntryAsync(spending.AccountEntryId, userId);
        
        var budgetDataDto = new BudgetDataApiDto {Spendings = new[] {spending.ToApiDto()}};
        if (accountEntry is not null)
            budgetDataDto.AccountEntries.Add(accountEntry.ToApiDto());
        
        return Results.Created("fillUrl", budgetDataDto);
    }
    
    public static async Task<IResult> AddAccount(IAccountService accountService, IUserService userService, AddNewAccountDto addNewAccountDto,  ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(ExtractAuthUserId(claims));
        var account = await accountService.AddNewAccountAsync(userId, addNewAccountDto.Name);
        var budgetDataDto = new BudgetDataApiDto {Accounts = new[] {account.ToApiDto()}};
        return Results.Created("fillUrl", budgetDataDto);
    }

    private static string ExtractAuthUserId(ClaimsPrincipal claims) => claims.FindFirstValue(ClaimTypes.NameIdentifier);
    public static IResult GetAll(IAccountService accountService, ClaimsPrincipal claims)
    {
        try
        {
            var userId = ExtractAuthUserId(claims);
            var accounts = accountService.GetAccounts().ToList();
            var accountDtos = accounts.Select(_ => _.ToApiDto());
            var accountEntryDtos =
                accounts.SelectMany(_ => accountService.GetAccountEntries(_.Id)).Select(_ => _.ToApiDto());
            var budgetaryItems = accountService.GetBudgetaryItems()?.ToList();

            var budgetaryItemDtos = new List<BudgetaryItemDto>();
            var budgetEntries = new List<BudgetEntryApiDto>();
            var spendings = new List<SpendingDto>();
            
            if (budgetaryItems != null)
            {
                budgetaryItemDtos.AddRange(budgetaryItems.Select(_ => _.ToApiDto()));
                var budgetChanges = budgetaryItems.SelectMany(_ => accountService.GetBudgetEntries(_.Id)
                    .Select(_ => BudgetEntryApiDto.ToApiDto(_)));
                budgetEntries.AddRange(budgetChanges);
            }

            spendings.AddRange(accountService.GetSpendings().Select(_ => _.ToApiDto()));

            var budgetDataDto = new BudgetDataApiDto
            {
                Accounts = accountDtos.ToList(),
                AccountEntries = accountEntryDtos.ToList(),
                BudgetaryItem = budgetaryItemDtos.ToList(),
                BudgetEntries = budgetEntries.ToList(),
                Spendings = spendings.ToList()
            };
            return Results.Ok(budgetDataDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}