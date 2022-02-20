namespace budget_backend.Controllers;

public static class Route
{
    public const string GetAllAccounts = "/GetAllAccounts";
    public const string AddAccount = "/AddAccount";
    public const string AddIncome = "/AddIncome";
    public const string GetAccountEntriesOfAccountBase = "/GetAccountEntriesOfAccount";
    public const string GetAccountEntriesOfAccount = GetAccountEntriesOfAccountBase  + "/{accountid}";
}