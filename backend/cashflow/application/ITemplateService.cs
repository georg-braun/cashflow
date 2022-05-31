using budget_backend.data;
using budget_backend.domain;
using budget_backend.domain.budget;

namespace budget_backend.application;

public interface ITemplateService
{
    IEnumerable<Template> GetTemplates(UserId userId);

    Task AddTemplateAsync(CategoryId commandCategoryId, TimeSpan commandInterval, double commandAmount,
        string commandNote, UserId userId);
}

internal class TemplateService : ITemplateService
{
    private readonly DataContext _dataContext;

    public TemplateService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IEnumerable<Template> GetTemplates(UserId userId)
    {
        return _dataContext.GetTemplates();
    }

    public async Task AddTemplateAsync(CategoryId categoryId, TimeSpan interval, double amount, string note,
        UserId userId)
    {
        var template = TemplateFactory.Create(categoryId, interval, amount, note);
        await _dataContext.AddTemplateAsync(template, userId);
    }
}