namespace budget_backend.Controllers;

public static class Routes
{
    
    public const string GetAllAccounts = "/GetAllAccounts";
    public const string GetAllBudgetaryItems = "/GetAllBudgetaryItems";
    public const string GetSpendings = "/GetSpendings";
    public const string AddAccount = "/AddAccount";
    public const string AddBudgetaryItem = "/AddBudgetaryItem";
    public const string AddIncome = "/AddIncome";
    public const string AddSpending = "/AddSpending";
    public const string SetBudgetEntry = "/SetBudgetEntry";
    public const string GetAccountEntriesOfAccountBase = "/GetAccountEntriesOfAccount";
    public const string GetAccountEntriesOfAccount = GetAccountEntriesOfAccountBase  + "/{accountId}";
    public const string GetBudgetChangesBase = "/GetBudgetChanges";
    public const string GetBudgetChanges = GetBudgetChangesBase  + "/{budgetaryItemId}";
    public const string GetAll = "/GetAll";
}