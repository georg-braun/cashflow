using System.Security.Claims;
using budget_backend.application;
using budget_backend.Controllers.apiDto;
using budget_backend.Controllers.apiDto.commands;
using budget_backend.Controllers.apiDto.datacontainer;
using budget_backend.domain.account;
using budget_backend.domain.budget;

namespace budget_backend.endpoints;

public static class AccountEndpoints
{
    public static async Task<IEnumerable<AccountApiDto>> GetAllAccounts(IAccountService accountService, IUserService userService, ClaimsPrincipal claims) 
    {
        var userId = await userService.GetUserIdAsync(EndpointUtilities.ExtractAuthUserId(claims));
        var accounts = accountService.GetAccounts(userId);
        return accounts.Select(_ => _.ToApiDto());
    }
    

    public static async Task<IEnumerable<AccountEntryApiDto>> GetAccountEntriesOfAccount(IAccountService accountService, IUserService userService, ClaimsPrincipal claims, string accountId)
    {
        var userId = await userService.GetUserIdAsync(EndpointUtilities.ExtractAuthUserId(claims));
        var accountGuid = AccountIdFactory.Create(Guid.Parse(accountId));
        var accountEntries = accountService.GetAccountEntries(accountGuid, userId);
        return accountEntries.Select(_ => _.ToApiDto());
    }

    public static async Task<IResult> AddAccountEntry(IAccountService accountService, IUserService userService, ClaimsPrincipal claims, AddAccountEntryCommand command)
    {
        var amount = command.Amount;
        var budgetaryItemId = command.BudgetaryItemId;

        if (amount >= 0.0 && budgetaryItemId != Guid.Empty)
            return Results.UnprocessableEntity(
                "Positive amount can't be associated with a budgetary item. A spending has a negative value");
        
        var userId = await userService.GetUserIdAsync(EndpointUtilities.ExtractAuthUserId(claims));
        var date = new DateOnly(command.Date.Year, command.Date.Month, command.Date.Day);
        var accountEntry = await accountService.AddAccountEntryAsync(AccountIdFactory.Create(command.AccountId), command.Amount, date, userId, BudgetaryItemIdFactory.Create(command.BudgetaryItemId));
        if (accountEntry != null)
        {
            var budgetDataDto = new BudgetDataApiDto {AccountEntries = new[] {accountEntry.ToApiDto()}};
            return Results.Created("fillUrl", budgetDataDto);
        }

        return Results.UnprocessableEntity();
    }
    
    public static async Task<IResult> AddAccount(IAccountService accountService, IUserService userService, AddNewAccountDto addNewAccountDto,  ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(EndpointUtilities.ExtractAuthUserId(claims));
        var account = await accountService.AddNewAccountAsync(userId, addNewAccountDto.Name);
        var budgetDataDto = new BudgetDataApiDto {Accounts = new[] {account.ToApiDto()}};
        return Results.Created("fillUrl", budgetDataDto);
    }
    
    public static async Task<IResult> GetAll(IAccountService accountService, IUserService userService, ClaimsPrincipal claims)
    {
        try
        {
            var userId = await userService.GetUserIdAsync(EndpointUtilities.ExtractAuthUserId(claims));
            var accounts = accountService.GetAccounts(userId).ToList();
            var accountDtos = accounts.Select(_ => _.ToApiDto());
            
            var accountEntryDtos =
                accounts.SelectMany(_ => accountService.GetAccountEntries(_.Id, userId)).Select(_ => _.ToApiDto());
            var budgetaryItems = accountService.GetBudgetaryItems(userId)?.ToList();

            var budgetaryItemDtos = new List<BudgetaryItemDto>();
            var budgetEntries = new List<BudgetEntryApiDto>();

            if (budgetaryItems != null)
            {
                budgetaryItemDtos.AddRange(budgetaryItems.Select(_ => _.ToApiDto()));
                var budgetChanges = budgetaryItems.SelectMany(_ => accountService.GetBudgetEntries(_.Id, userId)
                    .Select(_ => BudgetEntryApiDto.ToApiDto(_)));
                budgetEntries.AddRange(budgetChanges);
            }

            var budgetDataDto = new BudgetDataApiDto
            {
                Accounts = accountDtos.ToList(),
                AccountEntries = accountEntryDtos.ToList(),
                BudgetaryItems = budgetaryItemDtos.ToList(),
                BudgetEntries = budgetEntries.ToList(),
            };
            return Results.Ok(budgetDataDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task<IResult> DeleteAccount(IAccountService accountService, IUserService userService, DeleteAccountDto deleteAccountDto,  ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(EndpointUtilities.ExtractAuthUserId(claims));
        var accountIsDeleted = await accountService.DeleteAccountAsync(userId, deleteAccountDto.AccountId);

        BudgetDataApiDto budgetDataDto;
        budgetDataDto = accountIsDeleted ? new BudgetDataApiDto {DeletedAccountIds = new List<Guid>() {deleteAccountDto.AccountId}} : new BudgetDataApiDto();
            
        return Results.Created("fillUrl", budgetDataDto);
    }  
    
    public static async Task<IResult> DeleteBudgetaryItem(IAccountService accountService, IUserService userService, DeleteBudgetaryItemCommand deleteBudgetaryItem,  ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(EndpointUtilities.ExtractAuthUserId(claims));
        var budgetaryItemIsDeleted = await accountService.DeleteBudgetaryItemAsync(deleteBudgetaryItem.BudgetaryItemId, userId);

        BudgetDataApiDto budgetDataDto;
        budgetDataDto = budgetaryItemIsDeleted ? new BudgetDataApiDto {DeletedBudgetaryItemIds = new List<Guid>() {deleteBudgetaryItem.BudgetaryItemId}} : new BudgetDataApiDto();
            
        return Results.Created("fillUrl", budgetDataDto);
    }

    public static async Task<IResult> DeleteAccountEntry(IAccountService accountService, IUserService userService, DeleteAccountEntryDto deleteAccountEntry,  ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(EndpointUtilities.ExtractAuthUserId(claims));
        var accountEntryIsDeleted = await accountService.DeleteAccountEntryAsync(userId, deleteAccountEntry.AccountEntryId);

        BudgetDataApiDto budgetDataDto;
        budgetDataDto = accountEntryIsDeleted ? new BudgetDataApiDto {DeletedAccountEntryIds = new List<Guid>() {deleteAccountEntry.AccountEntryId}} : new BudgetDataApiDto();
            
        return Results.Created("fillUrl", budgetDataDto);
    }
}