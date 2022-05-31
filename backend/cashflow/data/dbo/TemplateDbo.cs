using System.ComponentModel.DataAnnotations;
using budget_backend.application;
using budget_backend.domain;
using budget_backend.domain.budget;

namespace budget_backend.data.dbo;

public class TemplateDbo
{
    [Key] public Guid Id { get; init; }

    public double Amount { get; init; }

    public TimeSpan Interval { get; init; }

    public string Note { get; init; } = string.Empty;

    public Guid CategoryId { get; init; }

    public Guid UserId { get; init; }
}

public static class TemplateDboExtensions
{
    public static TemplateDbo ToDbo(this Template template, UserId userId)
    {
        return new TemplateDbo()
        {
            Id = template.Id.Id,
            CategoryId = template.CategoryId.Id,
            Note = template.Note,
            Amount = template.Amount,
            Interval = template.Interval,
            UserId = userId.Id
        };
    }

    public static Template ToDomain(this TemplateDbo dbo)
    {
        return new Template(new TemplateId() {Id = dbo.Id}, dbo.Interval, CategoryIdFactory.Create(dbo.CategoryId),
            dbo.Amount, dbo.Note);
    }
}