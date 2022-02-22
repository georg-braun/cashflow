namespace budget_backend.Controllers;

public static class Route
{
    public const string GetAllAccounts = "/GetAllAccounts";
    public const string GetAllBudgetaryItems = "/GetAllBudgetaryItems";
    public const string AddAccount = "/AddAccount";
    public const string AddBudgetaryItem = "/AddBudgetaryItem";
    public const string AddIncome = "/AddIncome";
    public const string AddBudgetChange = "/AddBudgetChange";
    public const string GetAccountEntriesOfAccountBase = "/GetAccountEntriesOfAccount";
    public const string GetAccountEntriesOfAccount = GetAccountEntriesOfAccountBase  + "/{accountid}";
    public const string GetBudgetChangesBase = "/GetBudgetChanges";
    public const string GetBudgetChanges = GetBudgetChangesBase  + "/{budgetaryItemId}";
}