using budget_backend.data;
using budget_backend.data.dbDto;
using Microsoft.AspNetCore.Mvc;

namespace budget_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly DataContext _dataContext;

        public AccountController(ILogger<AccountController> logger, DataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        [HttpGet(Name = "GetAccounts")]
        public async Task<IActionResult> Get()
        {
            var accountDbDto = new Account() {Id = new Guid(), Name = "Cash"};
            await _dataContext.Accounts.AddAsync(accountDbDto);
            var result = _dataContext.Accounts.FirstOrDefault();
            await _dataContext.SaveChangesAsync();
            return Accepted();
        }
    }
}