using System.Security.Claims;
using budget_backend.application;
using budget_backend.Controllers.apiDto;
using budget_backend.Controllers.apiDto.commands;
using budget_backend.domain;
using budget_backend.domain.budget;

namespace budget_backend.Controllers;

public static class TemplateEndpoints
{
    public static async Task<IEnumerable<TemplateDto>> GetTemplates(ITemplateService moneyService,
        IUserService userService,
        ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(claims);
        var templates = moneyService.GetTemplates(userId);
        return templates.Select(_ => _.ToApiDto());
    }


    public static async Task<IResult> AddTemplate(AddTemplateCommand command, ITemplateService templateService,
        IUserService userService, ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(claims);
        var categoryId = CategoryIdFactory.Create(command.CategoryId);
        await templateService.AddTemplateAsync(categoryId, command.Interval, command.Amount, command.Note, userId);
        return Results.Ok();
    }
}