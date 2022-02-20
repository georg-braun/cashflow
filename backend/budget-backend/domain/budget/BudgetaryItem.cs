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
}