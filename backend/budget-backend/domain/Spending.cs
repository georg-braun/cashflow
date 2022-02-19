using budget_backend.domain.budget;

namespace budget_backend.domain;




public class Spending
{
    public Spending(Guid id, Account from, DateOnly timestamp, double amount, BudgetaryItem budgetaryItem)
    {
        Id = id;
        Amount = amount;
        Timestamp = timestamp;
        From = from;
        BudgetaryItem = budgetaryItem;

    }

    public Guid Id { get; }
    public double Amount { get; }
    public DateOnly Timestamp { get; }
    public Account From { get; }
    public Account To { get; }
    
    public BudgetaryItem BudgetaryItem { get; init; }
    
    
}