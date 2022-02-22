using budget_backend.application;
using budget_backend.Controllers.apiDto;
using budget_backend.Controllers.apiDto.commands;
using Microsoft.AspNetCore.Mvc;

namespace budget_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;

    public AccountController(ILogger<AccountController> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    [HttpGet(Route.GetAllAccounts)]
    public IEnumerable<AccountApiDto> GetAllAccounts()
    {
        var accounts =_accountService.GetAccounts();
        return accounts.Select(_ => _.ToApiDto());
    }
    
    [HttpGet(Route.GetAccountEntriesOfAccount)]
    public IEnumerable<AccountEntryApiDto> GetAccountEntriesOfAccount(string accountId)
    {
        var accountGuid = Guid.Parse(accountId);
        var accountEntries =_accountService.GetAccountEntries(accountGuid);
        return accountEntries.Select(_ => _.ToApiDto());
    }
    
    [HttpPost(Route.AddIncome)]
    public async Task<IActionResult> AddIncomeMethod([FromBody] AddIncomeDto addIncomeDto)
    {
        var date = new DateOnly(addIncomeDto.Timestamp.Year, addIncomeDto.Timestamp.Month, addIncomeDto.Timestamp.Day);
        var accountEntry = await _accountService.AddIncomeAsync(addIncomeDto.AccountId, addIncomeDto.Amount, date);
        return Created("fillUrl", accountEntry.ToApiDto());
    }
    
    [HttpPost(Route.AddAccount)]
    public async Task<IActionResult> AddAccount([FromBody] AddNewAccountDto addNewAccountDto)
    {
        var account = await _accountService.AddNewAccountAsync(addNewAccountDto.Name);
        return Created("fillUrl", account.ToApiDto());
    }
    
    [HttpPost(Route.AddBudgetaryItem)]
    public async Task<IActionResult> AddBudgetaryItem([FromBody] AddNewBudgetaryItemDto newBudgetaryItemDto)
    {
        var budgetaryItem = await _accountService.AddBudgetaryItemAsync(newBudgetaryItemDto.Name);
        return Created("fillUrl", budgetaryItem.ToApiDto());
    }
    
    [HttpGet(Route.GetAllBudgetaryItems)]
    public IEnumerable<BudgetaryItemDto> GetAllBudgetaryItems() => _accountService.GetBudgetaryItems().Select(_ => _.ToApiDto());
    
    [HttpGet(Route.GetBudgetChanges)]
    public IEnumerable<BudgetChangeApiDto> GetBudgetChanges(string budgetaryItemId)
    {
        var budgetaryItemGuid = Guid.Parse(budgetaryItemId);
        return _accountService.GetBudgetChanges(budgetaryItemGuid).Select(_ => _.ToApiDto());
    }


    [HttpPost(Route.AddBudgetChange)]
    public async Task<IActionResult> AddBudgetChange([FromBody] AddBudgetChangeDto addBudgetChangeDto)
    {
        var date = new DateOnly(addBudgetChangeDto.Date.Year, addBudgetChangeDto.Date.Month, addBudgetChangeDto.Date.Day);
        var budgetChange = await _accountService.AddBudgetChangeAsync(addBudgetChangeDto.BudgetaryItemId, addBudgetChangeDto.Amount, date);
        return Created("fillUrl", budgetChange.ToApiDto());
    }

}