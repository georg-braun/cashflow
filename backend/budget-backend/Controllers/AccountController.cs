using budget_backend.domain;
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

    [HttpGet(Name = "GetAccounts")]
    public async Task<apiDto.Account> Get()
    {
        var account =_accountService.GetAccountAsync("string");
        return new apiDto.Account() { Name = "Test-Account"};
    }
    
    [HttpPost(Name = "AddAccount")]
    public async Task<IActionResult> Post([FromBody] string name)
    {
        var account = AccountFactory.Create(name);
        account.AddEntry(40.50,DateOnlyExtensions.Today());
        account.AddEntry(60,DateOnlyExtensions.Today());
        await _accountService.AddNewAccount(account);
        
        return Created("fillUrl", new apiDto.Account());
    }
    
    /*
    [HttpPost(Name = "AddAccount")]
    public async Task<apiDto.Account> Get()
    {
        //var accountDbDto = new domain.Account {Id = new Guid(), Name = "Cash"};
        var account = AccountFactory.Create("Cash");
        account.AddTransaction(DateOnly.FromDateTime(DateTime.Now), 30);
        await _accountService.AddAccountAsync(account);

        return new apiDto.Account() { Name = "Test-Account"};
    }
    */
}