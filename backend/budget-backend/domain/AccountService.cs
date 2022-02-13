using AutoMapper;
using budget_backend.data;

namespace budget_backend.domain;

public interface IAccountService
{
    Task AddAsync(Account account);
}

public class AccountService : IAccountService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public AccountService(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task AddAsync(Account account)
    {
        var dtoAccount = _mapper.Map<data.dbDto.Account>(account);
        await _dataContext.Accounts.AddAsync(dtoAccount);
        await _dataContext.SaveChangesAsync();
    }
    
/*
    public bool TryGet(Guid id, out Account account)
    {
        var foundAccount = _accounts.FirstOrDefault(_ => _.Id.Equals(id));
        account = foundAccount ?? GetDefaultAccount();
        return foundAccount != null;
    }

    public bool TryGet(string accountName, out Account account)
    {
        var foundAccount = _accounts.FirstOrDefault(_ => _.Name.Equals(accountName));
        account = foundAccount ?? GetDefaultAccount();
        return foundAccount != null;
    }

    public void Update(Account account)
    {
        Delete(account);
        _accounts.Add(account);
    }

    public void Delete(Account account)
    {
        _accounts.RemoveAll(_ => _.Id.Equals(account.Id));
    }
    */
}

public static class AccountFactory
{
    public static Account Create(string name)
    {
        var id = new Guid();
        return new Account(id, name);
    }
}