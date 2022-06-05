using System.Security.Claims;
using budget_backend.application;
using budget_backend.Controllers.apiDto;
using budget_backend.Controllers.apiDto.commands;
using budget_backend.data.dbo;
using budget_backend.Domain.Commands;
using budget_backend.Domain.Queries;
using MediatR;

namespace budget_backend.Controllers;

public static class TemplateEndpoints
{
    public static async Task<IEnumerable<TemplateDto>> GetTemplates(IMediator mediator,
        IUserService userService,
        ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(claims);
        var templates = await mediator.Send(new GetTemplatesQuery(userId));
        return templates.Select(_ => _.ToApiDto());
    }
    
    
    public static async Task<IResult> AddTemplate(AddTemplateCommand command, IMediator mediator,
        IUserService userService, ClaimsPrincipal claims)
    {
        var userId = await userService.GetUserIdAsync(claims);
        var categoryId = CategoryIdFactory.Create(command.CategoryId);
        var template = TemplateFactory.Create(categoryId, command.Interval, command.Amount, command.Note, userId);
        await mediator.Send(new CreateTemplateCommand(template));
        return Results.Ok();
    }
}