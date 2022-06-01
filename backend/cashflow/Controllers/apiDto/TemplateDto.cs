using budget_backend.domain;

namespace budget_backend.Controllers.apiDto;

public record TemplateDto(Guid Id, TimeSpan Interval, Guid CategoryId, double Amount, string Note);

public static class TemplateDtoExtensions
{
    public static TemplateDto ToApiDto(this Template template)
    {
        return new TemplateDto(
            template.Id.Id,
            template.Interval,
            template.CategoryId.Id,
            template.Amount,
            template.Note);
    }
}