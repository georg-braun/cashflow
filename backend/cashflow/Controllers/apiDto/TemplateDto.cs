using budget_backend.data.dbo;

namespace budget_backend.Controllers.apiDto;

public record TemplateDto(Guid Id, TimeSpan Interval, Guid CategoryId, double Amount, string Note);

public static class TemplateDtoExtensions
{
    public static TemplateDto ToApiDto(this Template template)
    {
        return new TemplateDto(
            template.Id,
            template.Interval,
            template.CategoryId,
            template.Amount,
            template.Note);
    }
}