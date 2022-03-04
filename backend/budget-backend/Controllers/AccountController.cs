using budget_backend.application;
using budget_backend.Controllers.apiDto;
using budget_backend.Controllers.apiDto.commands;
using budget_backend.Controllers.apiDto.datacontainer;
using budget_backend.domain.account;
using budget_backend.domain.budget;
using Microsoft.AspNetCore.Mvc;

namespace budget_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    [HttpGet(Route.GetAll)]
    public BudgetDataApiDto GetAll()
    {
        var accounts = _accountService.GetAccounts();
        var accountDtos = accounts.Select(_ => _.ToApiDto());
        var accountEntryDtos =
            accounts.SelectMany(_ => _accountService.GetAccountEntries(_.Id)).Select(_ => _.ToApiDto());
        var budgetaryItems = _accountService.GetBudgetaryItems().ToList();
        var budgetaryItemDtos = budgetaryItems.Select(_ => _.ToApiDto());
        var budgetChangeDtos = budgetaryItems.SelectMany(_ => _accountService.GetBudgetEntries(_.Id)
            .Select(_ => BudgetEntryApiDto.ToApiDto(_)));
        var spendingDtos = _accountService.GetSpendings().Select(_ => _.ToApiDto());

        var budgetDataDto = new BudgetDataApiDto
        {
            Accounts = accountDtos.ToList(), 
            AccountEntries = accountEntryDtos.ToList(), 
            BudgetaryItem = budgetaryItemDtos.ToList(),
            BudgetEntries = budgetChangeDtos.ToList(), 
            Spendings = spendingDtos.ToList()
        };
        return budgetDataDto;
    }

    [HttpGet(Route.GetAllAccounts)]
    public IEnumerable<AccountApiDto> GetAllAccounts()
    {
        var accounts = _accountService.GetAccounts();
        return accounts.Select(_ => _.ToApiDto());
    }

    [HttpGet(Route.GetAccountEntriesOfAccount)]
    public IEnumerable<AccountEntryApiDto> GetAccountEntriesOfAccount(string accountId)
    {
        var accountGuid = AccountIdFactory.Create(Guid.Parse(accountId));
        var accountEntries = _accountService.GetAccountEntries(accountGuid);
        return accountEntries.Select(_ => _.ToApiDto());
    }

    [HttpGet(Route.GetSpendings)]
    public IEnumerable<SpendingDto> GetSpendings()
    {
        var spendings = _accountService.GetSpendings();
        return spendings.Select(_ => _.ToApiDto());
    }

    [HttpPost(Route.AddIncome)]
    public async Task<IActionResult> AddIncome([FromBody] AddIncomeDto addIncomeDto)
    {
        var date = new DateOnly(addIncomeDto.Date.Year, addIncomeDto.Date.Month, addIncomeDto.Date.Day);
        var accountEntry = await _accountService.AddIncomeAsync(AccountIdFactory.Create(addIncomeDto.AccountId), addIncomeDto.Amount, date);
        if (accountEntry != null)
        {
            var budgetDataDto = new BudgetDataApiDto {AccountEntries = new[] {accountEntry.ToApiDto()}};
            return Created("fillUrl", budgetDataDto);
        }

        return UnprocessableEntity();
    }

    [HttpPost(Route.AddSpending)]
    public async Task<IActionResult> AddSpending([FromBody] AddSpending req)
    {
        var date = new DateOnly(req.Date.Year, req.Date.Month, req.Date.Day);
        var accountId = AccountIdFactory.Create(req.AccountId);
        var budgetaryItemId = BudgetaryItemIdFactory.Create(req.BudgetaryItemId);
        var spending = await _accountService.AddSpendingAsync(accountId, budgetaryItemId, req.Amount, date);

        if (spending is null)
            return UnprocessableEntity();
        
        var accountEntry = await _accountService.GetAccountEntryAsync(spending.AccountEntryId);
        
        var budgetDataDto = new BudgetDataApiDto {Spendings = new[] {spending.ToApiDto()}};
        if (accountEntry is not null)
            budgetDataDto.AccountEntries.Add(accountEntry.ToApiDto());
        
        return Created("fillUrl", budgetDataDto);
    }

    [HttpPost(Route.AddAccount)]
    public async Task<IActionResult> AddAccount([FromBody] AddNewAccountDto addNewAccountDto)
    {
        var account = await _accountService.AddNewAccountAsync(addNewAccountDto.Name);
        var budgetDataDto = new BudgetDataApiDto {Accounts = new[] {account.ToApiDto()}};
        return Created("fillUrl", budgetDataDto);
    }

    [HttpPost(Route.AddBudgetaryItem)]
    public async Task<IActionResult> AddBudgetaryItem([FromBody] AddNewBudgetaryItemDto newBudgetaryItemDto)
    {
        var budgetaryItem = await _accountService.AddBudgetaryItemAsync(newBudgetaryItemDto.Name);
        var budgetDataDto = new BudgetDataApiDto {BudgetaryItem = new[] {budgetaryItem.ToApiDto()}};
        return Created("fillUrl", budgetDataDto);
    }

    [HttpGet(Route.GetAllBudgetaryItems)]
    public IEnumerable<BudgetaryItemDto> GetAllBudgetaryItems()
    {
        return _accountService.GetBudgetaryItems().Select(_ => _.ToApiDto());
    }

    [HttpGet(Route.GetBudgetChanges)]
    public IEnumerable<BudgetEntryApiDto> GetBudgetChanges(string budgetaryItemId)
    {
        var typedBudgetaryItemId = BudgetaryItemIdFactory.Create(Guid.Parse(budgetaryItemId));
        return _accountService.GetBudgetEntries(typedBudgetaryItemId).Select(BudgetEntryApiDto.ToApiDto);
    }


    [HttpPost(Route.SetBudgetEntry)]
    public async Task<IActionResult> AddBudgetEntry([FromBody] SetBudgetEntryDto item)
    {
        var budgetaryItemId = BudgetaryItemIdFactory.Create(item.BudgetaryItemid);
        var budgetEntry = await _accountService.AddBudgetEntryAsync(budgetaryItemId, item.Amount, item.Month);
        var budgetDataDto = new BudgetDataApiDto {BudgetEntries = new[] {BudgetEntryApiDto.ToApiDto(budgetEntry)}};
        return Created("fillUrl", budgetDataDto);
    }
}