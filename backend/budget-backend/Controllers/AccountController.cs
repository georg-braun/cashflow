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
        var accountFound =_accountService.GetAccountAsync("DKB", out var account);
        return new apiDto.Account() { Name = "Test-Account"};
    }
    
    [HttpPost(Name = "AddAccount")]
    public async Task<IActionResult> Post([FromBody] string name)
    {
        var account = AccountFactory.Create(name);
        account.AddTransaction(DateOnlyExtensions.Today(), 40.50);
        await _accountService.AddAccountAsync(account);
        
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