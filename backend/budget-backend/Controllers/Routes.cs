namespace budget_backend.Controllers;

public static class Routes
{
    private const string Base = "/api";
    private const string Version = "";
    
    public const string GetAllAccounts = $"{Base}{Version}/GetAllAccounts";
    public const string GetAllBudgetaryItems = $"{Base}{Version}/GetAllBudgetaryItems";
    
    public const string AddAccount = $"{Base}{Version}/AddAccount";
    public const string DeleteAccount = $"{Base}{Version}/DeleteAccount";
    
    public const string DeleteAccountEntry = $"{Base}{Version}/DeleteAccountEntry";
    
    public const string AddBudgetaryItem = $"{Base}{Version}/AddBudgetaryItem";
    public const string DeleteBudgetryItem = $"{Base}{Version}/DeleteBudgetaryItem";
    
    
    public const string AddAccountEntry = $"{Base}{Version}/AddAccountEntry";
    public const string SetBudgetEntry = $"{Base}{Version}/SetBudgetEntry";
    public const string GetAccountEntriesOfAccountBase = $"{Base}{Version}/GetAccountEntriesOfAccount";
    public const string GetAccountEntriesOfAccount = GetAccountEntriesOfAccountBase + "/{accountId}";
    public const string GetBudgetChangesBase = $"{Base}{Version}/GetBudgetChanges";
    public const string GetBudgetChanges = GetBudgetChangesBase + "/{budgetaryItemId}";
    public const string GetAll = $"{Base}{Version}/GetAll";
}