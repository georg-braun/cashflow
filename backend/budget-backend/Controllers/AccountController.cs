using budget_backend.data;
using budget_backend.data.dbDto;
using Microsoft.AspNetCore.Mvc;

namespace budget_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly DataContext _dataContext;
    private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger, DataContext dataContext)
    {
        _logger = logger;
        _dataContext = dataContext;
    }

    [HttpGet(Name = "GetAccounts")]
    public async Task<apiDto.Account> Get()
    {
        var accountDbDto = new Account {Id = new Guid(), Name = "Cash"};
        await _dataContext.Accounts.AddAsync(accountDbDto);
        await _dataContext.SaveChangesAsync();
        var result = _dataContext.Accounts.FirstOrDefault();
        
        return new apiDto.Account() { Name = "Test-Account"};
    }
}