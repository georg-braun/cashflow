using System.ComponentModel.DataAnnotations;
using budget_backend.application;

namespace budget_backend.data.dbo;

public class Template
{
    [Key] public Guid Id { get; init; }

    public double Amount { get; init; }

    public TimeSpan Interval { get; init; }

    public string Note { get; init; } = string.Empty;

    public Guid CategoryId { get; init; }

    public Guid UserId { get; init; }
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
    public static Template Create(CategoryId categoryId, TimeSpan interval, double amount, string note, UserId userId)
    {
        return new Template
        {
            Id = TemplateIdFactory.CreateNew().Id,
            Interval = interval,
            CategoryId = categoryId.Id,
            Amount = amount,
            Note = note,
            UserId = userId.Id
        };
    }
}