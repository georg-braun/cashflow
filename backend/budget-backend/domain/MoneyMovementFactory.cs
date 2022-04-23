using budget_backend.domain.budget;

namespace budget_backend.domain;

public static class MoneyMovementFactory
{
    public static MoneyMovement Create(double amount, DateTime date, string note, Guid categoryId)
    {
        return new MoneyMovement(MoneyMovementIdFactory.CreateNew(), amount, date, note, CategoryIdFactory.Create(categoryId));
    } 
    

    public static MoneyMovement Create(double amount, DateTime date, string note, CategoryId categoryId)
    {
        return new MoneyMovement(MoneyMovementIdFactory.CreateNew(), amount, date, note, categoryId);
    }
}