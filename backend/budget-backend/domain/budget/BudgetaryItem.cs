using System.ComponentModel.DataAnnotations;

namespace budget_backend.domain.budget;

public class BudgetaryItem
{
    public BudgetaryItem(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }

    public string Name { get; }

    private List<BudgetChange> _budgetChanges = new();
    private List<Spending> _spendings = new();

    public double Remaining()
    {
        return _budgetChanges.Sum(_ => _.Amount) - _spendings.Sum(_ => _.Amount);
    }

    public void AddSpending(Spending spending)
    {
        _spendings.Add(spending);
    }

    public void AddChange(double amount, DateOnly timestamp)
    {
        var budgetChange = BudgetFactory.CreateBudgetChange(this, amount, timestamp);
        _budgetChanges.Add(budgetChange);
    }
}