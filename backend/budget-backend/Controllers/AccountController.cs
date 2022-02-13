using budget_backend.data;
using budget_backend.domain;
using Microsoft.AspNetCore.Mvc;
using Account = budget_backend.data.dbDto.Account;

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
        //var accountDbDto = new domain.Account {Id = new Guid(), Name = "Cash"};
        var account = AccountFactory.Create("Cash");
        await _accountService.AddAsync(account);
        /*
        await _dataContext.SaveChangesAsync();
        var result = _dataContext.Accounts.FirstOrDefault();
        */
        return new apiDto.Account() { Name = "Test-Account"};
    }
}