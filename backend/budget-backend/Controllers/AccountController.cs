using budget_backend.application;
using budget_backend.Controllers.apiDto;
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

    [HttpGet(Name = "GetAllAccounts")]
    public IEnumerable<AccountApiDto> Get()
    {
        var accounts =_accountService.GetAccounts();
        return accounts.Select(_ => _.ToApiDto());

    }
    
    [HttpPost(Name = "AddAccount")]
    public async Task<IActionResult> Post([FromBody] AddNewAccountDto addNewAccountDto)
    {
        await _accountService.AddNewAccountAsync(addNewAccountDto.Name);
        return Created("fillUrl", new AccountApiDto());
    }
    

}