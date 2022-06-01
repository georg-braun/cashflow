using budget_backend.domain.budget;

namespace budget_backend.domain;

public class Template
{
    public Template(TemplateId id, TimeSpan interval, CategoryId categoryId, double amount, string note)
    {
        Id = id;
        Interval = interval;
        CategoryId = categoryId;
        Amount = amount;
        Note = note;
    }

    public TemplateId Id { get; }
    public TimeSpan Interval { get; }
    public CategoryId CategoryId { get; }
    public double Amount { get; }
    public string Note { get; }
}

public class TemplateId
{
    public Guid Id { get; init; }
}

public static class TemplateIdFactory
{
    public static TemplateId Create(Guid id)
    {
        return new() {Id = id};
    }

    public static TemplateId CreateNew()
    {
        return Create(Guid.NewGuid());
    }
}

public static class TemplateFactory
{
    public static Template Create(CategoryId categoryId, TimeSpan interval, double amount, string note)
    {
        return new Template(TemplateIdFactory.CreateNew(), interval, categoryId, amount, note);
    }
}