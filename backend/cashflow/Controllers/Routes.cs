namespace budget_backend.Controllers;

public static class Routes
{
    private const string Base = "/api";
    private const string Version = "";

    public const string GetAll = $"{Base}{Version}/GetAll";
    public const string GetMoneyMovements = $"{Base}{Version}/GetMoneyMovements";
    public const string AddMoneyMovement = $"{Base}{Version}/AddMoneyMovement";
    public const string DeleteMoneyMovement = $"{Base}{Version}/DeleteMoneyMovement";

    public const string GetCategories = $"{Base}{Version}/GetCategories";
    public const string AddCategory = $"{Base}{Version}/AddCategory";
    public const string DeleteCategory = $"{Base}{Version}/DeleteCategory";
    
    public const string GetTemplates = $"{Base}{Version}/GetTemplates";
    public const string AddTemplate = $"{Base}{Version}/AddTemplate";
}