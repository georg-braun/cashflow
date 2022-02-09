namespace budget_backend.domain;

public class TransactionService
{
    private AccountService _accountService;
    private List<Transaction> _transactions = new();

    public TransactionService(AccountService accountService)
    {
        _accountService = accountService;
    }

    public bool AddTransaction(DateTime timestamp, double amount, Guid accountId)
    {
        var accountFound = _accountService.TryGetAccount(accountId, out var account);
        if (!accountFound) return false;
        
        var transaction = TransactionFactory.Create(timestamp, amount, account);
        _transactions.Add(transaction);
        return true;
    }

    public void DeleteTransaction(Guid id)
    {
        _transactions.RemoveAll(_ => _.Id.Equals(id));
    }

    public double GetTotalBalance()
    {
        return _transactions.Sum(_ => _.Amount);
    }
}

public static class TransactionFactory
{
    public static Transaction Create(DateTime timestamp, double amount, Account account)
    {
        var id = new Guid();
        return new Transaction(id, timestamp, amount, account);
    }
}